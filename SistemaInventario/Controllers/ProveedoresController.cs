using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SistemaInventario.Data;
using SistemaInventario.Models;

namespace SistemaInventario.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index(string buscar)
        {
            var proveedores = _context.Proveedores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                proveedores = proveedores.Where(p => p.Nombre.Contains(buscar));
            }

            return View(proveedores.OrderBy(p => p.Nombre).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }

            return View(proveedor);
        }

        public ActionResult Create()
        {
            return View(new Proveedor { Activo = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedores.Add(proveedor);
                _context.SaveChanges();
                TempData["Mensaje"] = "Proveedor creado correctamente.";
                return RedirectToAction("Index");
            }

            return View(proveedor);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }

            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(proveedor).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["Mensaje"] = "Proveedor actualizado correctamente.";
                return RedirectToAction("Index");
            }

            return View(proveedor);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }

            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                _context.SaveChanges();
                TempData["Mensaje"] = "Proveedor eliminado correctamente.";
            }

            return RedirectToAction("Index");
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
