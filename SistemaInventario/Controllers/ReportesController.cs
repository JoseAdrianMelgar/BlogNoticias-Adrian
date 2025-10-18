using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SistemaInventario.Data;
using SistemaInventario.Models;
using SistemaInventario.ViewModels;

namespace SistemaInventario.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Inventario(ReporteInventarioFiltro filtro)
        {
            var viewModel = new ReporteInventarioViewModel
            {
                Filtro = filtro ?? new ReporteInventarioFiltro()
            };

            if (string.IsNullOrEmpty(viewModel.Filtro.AgruparPor))
            {
                viewModel.Filtro.AgruparPor = "dia";
            }

            var movimientos = ObtenerMovimientosFiltrados(viewModel.Filtro)
                .OrderBy(m => m.Fecha)
                .ToList();

            viewModel.Movimientos = movimientos;
            viewModel.TotalEntradas = movimientos.Where(m => m.Tipo == TipoMovimiento.Entrada).Sum(m => m.Cantidad);
            viewModel.TotalSalidas = movimientos.Where(m => m.Tipo == TipoMovimiento.Salida).Sum(m => m.Cantidad);

            viewModel.Productos = new SelectList(_context.Productos.OrderBy(p => p.Nombre), "ProductoId", "Nombre", filtro?.ProductoId);
            viewModel.Proveedores = new SelectList(_context.Proveedores.OrderBy(p => p.Nombre), "ProveedorId", "Nombre", filtro?.ProveedorId);

            ConstruirDatosGrafico(viewModel);

            return View(viewModel);
        }

        public FileResult ExportarCsv(DateTime? fechaDesde, DateTime? fechaHasta, int? productoId, int? proveedorId, string agruparPor)
        {
            var filtro = new ReporteInventarioFiltro
            {
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                ProductoId = productoId,
                ProveedorId = proveedorId,
                AgruparPor = agruparPor
            };

            var movimientos = ObtenerMovimientosFiltrados(filtro)
                .OrderBy(m => m.Fecha)
                .ToList();

            var builder = new StringBuilder();
            builder.AppendLine("Fecha,Producto,Tipo,Proveedor,Cantidad,Observaciones");
            foreach (var movimiento in movimientos)
            {
                var proveedor = movimiento.Proveedor != null ? movimiento.Proveedor.Nombre : string.Empty;
                builder.AppendLine(string.Format("{0},{1},{2},{3},{4},\"{5}\"",
                    movimiento.Fecha.ToString("yyyy-MM-dd"),
                    movimiento.Producto.Nombre,
                    movimiento.Tipo,
                    proveedor,
                    movimiento.Cantidad,
                    (movimiento.Observaciones ?? string.Empty).Replace("\"", "''")));
            }

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(bytes, "text/csv", "reporte-inventario.csv");
        }

        private IQueryable<MovimientoInventario> ObtenerMovimientosFiltrados(ReporteInventarioFiltro filtro)
        {
            var query = _context.MovimientosInventario
                .Include(m => m.Producto)
                .Include(m => m.Proveedor)
                .AsQueryable();

            if (filtro == null)
            {
                return query;
            }

            if (filtro.FechaDesde.HasValue)
            {
                query = query.Where(m => DbFunctions.TruncateTime(m.Fecha) >= DbFunctions.TruncateTime(filtro.FechaDesde.Value));
            }

            if (filtro.FechaHasta.HasValue)
            {
                query = query.Where(m => DbFunctions.TruncateTime(m.Fecha) <= DbFunctions.TruncateTime(filtro.FechaHasta.Value));
            }

            if (filtro.ProductoId.HasValue)
            {
                query = query.Where(m => m.ProductoId == filtro.ProductoId.Value);
            }

            if (filtro.ProveedorId.HasValue)
            {
                query = query.Where(m => m.ProveedorId == filtro.ProveedorId.Value);
            }

            return query;
        }

        private void ConstruirDatosGrafico(ReporteInventarioViewModel viewModel)
        {
            var serializer = new JavaScriptSerializer();
            string agruparPor = viewModel.Filtro?.AgruparPor ?? "dia";

            if (viewModel.Movimientos == null || !viewModel.Movimientos.Any())
            {
                viewModel.ChartLabelsJson = serializer.Serialize(new string[0]);
                viewModel.ChartDataJson = serializer.Serialize(new int[0]);
                viewModel.ChartTitle = "Sin datos";
                return;
            }

            if (agruparPor == "producto")
            {
                var datos = viewModel.Movimientos
                    .GroupBy(m => m.Producto.Nombre)
                    .Select(g => new
                    {
                        Etiqueta = g.Key,
                        Cantidad = g.Sum(m => m.Tipo == TipoMovimiento.Entrada ? m.Cantidad : -m.Cantidad)
                    })
                    .OrderByDescending(x => Math.Abs(x.Cantidad))
                    .ToList();

                viewModel.ChartLabelsJson = serializer.Serialize(datos.Select(d => d.Etiqueta).ToArray());
                viewModel.ChartDataJson = serializer.Serialize(datos.Select(d => d.Cantidad).ToArray());
                viewModel.ChartTitle = "Balance por producto";
            }
            else
            {
                var datos = viewModel.Movimientos
                    .GroupBy(m => m.Fecha.Date)
                    .Select(g => new
                    {
                        Etiqueta = g.Key,
                        Cantidad = g.Sum(m => m.Tipo == TipoMovimiento.Entrada ? m.Cantidad : -m.Cantidad)
                    })
                    .OrderBy(d => d.Etiqueta)
                    .ToList();

                viewModel.ChartLabelsJson = serializer.Serialize(datos.Select(d => d.Etiqueta.ToString("yyyy-MM-dd")).ToArray());
                viewModel.ChartDataJson = serializer.Serialize(datos.Select(d => d.Cantidad).ToArray());
                viewModel.ChartTitle = "Balance diario";
            }
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
