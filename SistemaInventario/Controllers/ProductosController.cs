using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SistemaInventario.Data;
using SistemaInventario.Models;

namespace SistemaInventario.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index(string buscar, int? page)
        {
            var productos = _context.Productos.Include(p => p.Proveedor).AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                productos = productos.Where(p => p.Nombre.Contains(buscar));
            }

            const int pageSize = 10;
            int pageNumber = page ?? 1;
            int total = productos.Count();

            var items = productos
                .OrderBy(p => p.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Total = total;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            ViewBag.Buscar = buscar;

            return View(items);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = _context.Productos.Include(p => p.Proveedor).FirstOrDefault(p => p.ProductoId == id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        public ActionResult Create()
        {
            ViewBag.ProveedorId = new SelectList(_context.Proveedores.OrderBy(p => p.Nombre), "ProveedorId", "Nombre");
            return View(new Producto { Activo = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producto producto)
        {
            if (_context.Productos.Any(p => p.Nombre == producto.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe un producto con ese nombre.");
            }

            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                TempData["Mensaje"] = "Producto creado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.ProveedorId = new SelectList(_context.Proveedores.OrderBy(p => p.Nombre), "ProveedorId", "Nombre", producto.ProveedorId);
            return View(producto);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProveedorId = new SelectList(_context.Proveedores.OrderBy(p => p.Nombre), "ProveedorId", "Nombre", producto.ProveedorId);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Producto producto)
        {
            if (_context.Productos.Any(p => p.ProductoId != producto.ProductoId && p.Nombre == producto.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe un producto con ese nombre.");
            }

            if (ModelState.IsValid)
            {
                _context.Entry(producto).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["Mensaje"] = "Producto actualizado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.ProveedorId = new SelectList(_context.Proveedores.OrderBy(p => p.Nombre), "ProveedorId", "Nombre", producto.ProveedorId);
            return View(producto);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = _context.Productos.Include(p => p.Proveedor).FirstOrDefault(p => p.ProductoId == id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
                TempData["Mensaje"] = "Producto eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult StockBajo()
        {
            var productos = _context.Productos
                .Include(p => p.Proveedor)
                .Where(p => p.StockActual <= p.StockMinimo)
                .OrderBy(p => p.Nombre)
                .ToList();

            return View(productos);
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
