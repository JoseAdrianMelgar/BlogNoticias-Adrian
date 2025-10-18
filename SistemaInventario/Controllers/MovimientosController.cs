using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SistemaInventario.Data;
using SistemaInventario.Models;
using SistemaInventario.ViewModels;

namespace SistemaInventario.Controllers
{
    public class MovimientosController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index(DateTime? fechaDesde, DateTime? fechaHasta, int? productoId, int? proveedorId)
        {
            var movimientos = _context.MovimientosInventario
                .Include(m => m.Producto)
                .Include(m => m.Proveedor)
                .AsQueryable();

            if (fechaDesde.HasValue)
            {
                movimientos = movimientos.Where(m => DbFunctions.TruncateTime(m.Fecha) >= DbFunctions.TruncateTime(fechaDesde.Value));
            }

            if (fechaHasta.HasValue)
            {
                movimientos = movimientos.Where(m => DbFunctions.TruncateTime(m.Fecha) <= DbFunctions.TruncateTime(fechaHasta.Value));
            }

            if (productoId.HasValue)
            {
                movimientos = movimientos.Where(m => m.ProductoId == productoId.Value);
            }

            if (proveedorId.HasValue)
            {
                movimientos = movimientos.Where(m => m.ProveedorId == proveedorId.Value);
            }

            ViewBag.ProductoId = new SelectList(_context.Productos.OrderBy(p => p.Nombre), "ProductoId", "Nombre", productoId);
            ViewBag.ProveedorId = new SelectList(_context.Proveedores.OrderBy(p => p.Nombre), "ProveedorId", "Nombre", proveedorId);
            ViewBag.FechaDesde = fechaDesde?.ToString("yyyy-MM-dd");
            ViewBag.FechaHasta = fechaHasta?.ToString("yyyy-MM-dd");

            return View(movimientos.OrderByDescending(m => m.Fecha).ToList());
        }

        public ActionResult Create(TipoMovimiento? tipo, int? productoId)
        {
            var viewModel = CrearViewModel();
            if (tipo.HasValue)
            {
                viewModel.Tipo = tipo.Value;
            }

            if (productoId.HasValue)
            {
                viewModel.ProductoId = productoId.Value;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovimientoInventarioViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(CrearViewModel(viewModel));
            }

            var producto = _context.Productos.SingleOrDefault(p => p.ProductoId == viewModel.ProductoId);
            if (producto == null)
            {
                ModelState.AddModelError("ProductoId", "El producto seleccionado no existe.");
                return View(CrearViewModel(viewModel));
            }

            if (viewModel.Tipo == TipoMovimiento.Entrada && !viewModel.ProveedorId.HasValue)
            {
                ModelState.AddModelError("ProveedorId", "Seleccione un proveedor para las entradas.");
                return View(CrearViewModel(viewModel));
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (viewModel.Tipo == TipoMovimiento.Entrada)
                    {
                        producto.StockActual += viewModel.Cantidad;
                    }
                    else
                    {
                        if (producto.StockActual - viewModel.Cantidad < 0)
                        {
                            ModelState.AddModelError("Cantidad", "No hay stock suficiente para realizar la salida.");
                            transaction.Rollback();
                            return View(CrearViewModel(viewModel));
                        }

                        producto.StockActual -= viewModel.Cantidad;
                        viewModel.ProveedorId = null;
                    }

                    var movimiento = new MovimientoInventario
                    {
                        ProductoId = viewModel.ProductoId,
                        Tipo = viewModel.Tipo,
                        Cantidad = viewModel.Cantidad,
                        Fecha = viewModel.Fecha == default(DateTime) ? DateTime.Now : viewModel.Fecha,
                        ProveedorId = viewModel.Tipo == TipoMovimiento.Entrada ? viewModel.ProveedorId : null,
                        Observaciones = viewModel.Observaciones
                    };

                    _context.MovimientosInventario.Add(movimiento);
                    _context.Entry(producto).State = EntityState.Modified;
                    _context.SaveChanges();

                    transaction.Commit();
                    TempData["Mensaje"] = "Movimiento registrado correctamente.";
                    return RedirectToAction("Index");
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al registrar el movimiento.");
                    return View(CrearViewModel(viewModel));
                }
            }
        }

        private MovimientoInventarioViewModel CrearViewModel(MovimientoInventarioViewModel baseModel = null)
        {
            var model = baseModel ?? new MovimientoInventarioViewModel();
            model.Productos = _context.Productos
                .OrderBy(p => p.Nombre)
                .Select(p => new SelectListItem
                {
                    Value = p.ProductoId.ToString(),
                    Text = p.Nombre,
                    Selected = model.ProductoId == p.ProductoId
                });

            model.Proveedores = _context.Proveedores
                .OrderBy(p => p.Nombre)
                .Select(p => new SelectListItem
                {
                    Value = p.ProveedorId.ToString(),
                    Text = p.Nombre,
                    Selected = model.ProveedorId == p.ProveedorId
                });

            return model;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
