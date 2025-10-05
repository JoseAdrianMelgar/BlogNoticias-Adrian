using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogNoticias.DAL;
using BlogNoticias.Models;

namespace BlogNoticias.Controllers
{
    public class PublicacionesController : Controller
    {
        private readonly BlogContext _db = new BlogContext();

        public ActionResult Index(int? categoriaId)
        {
            var publicaciones = _db.Publicaciones
                .Include(p => p.Autor)
                .Include(p => p.Categoria)
                .AsQueryable();

            if (categoriaId.HasValue)
            {
                publicaciones = publicaciones.Where(p => p.CategoriaId == categoriaId.Value);
            }

            ViewBag.Categorias = _db.Categorias.OrderBy(c => c.Nombre).ToList();

            return View(publicaciones.OrderByDescending(p => p.FechaPublicacion).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var publicacion = _db.Publicaciones
                .Include(p => p.Autor)
                .Include(p => p.Categoria)
                .Include(p => p.Comentarios.Select(c => c.Autor))
                .Include(p => p.Comentarios.Select(c => c.Publicacion.Autor))
                .SingleOrDefault(p => p.Id == id.Value);

            if (publicacion == null)
            {
                return HttpNotFound();
            }

            return View(publicacion);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(_db.Categorias.OrderBy(c => c.Nombre), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Titulo,Contenido,CategoriaId")] Publicacion publicacion)
        {
            var usuario = GetUsuarioActual();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                publicacion.AutorId = usuario.Id;
                publicacion.FechaPublicacion = DateTime.UtcNow;
                _db.Publicaciones.Add(publicacion);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(_db.Categorias.OrderBy(c => c.Nombre), "Id", "Nombre", publicacion.CategoriaId);
            return View(publicacion);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var publicacion = _db.Publicaciones.Find(id);
            if (publicacion == null)
            {
                return HttpNotFound();
            }

            var usuario = GetUsuarioActual();
            if (usuario == null || (!usuario.EsAdministrador && publicacion.AutorId != usuario.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ViewBag.CategoriaId = new SelectList(_db.Categorias.OrderBy(c => c.Nombre), "Id", "Nombre", publicacion.CategoriaId);
            return View(publicacion);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Titulo,Contenido,CategoriaId")] Publicacion publicacion)
        {
            var usuario = GetUsuarioActual();
            if (usuario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var existente = _db.Publicaciones.SingleOrDefault(p => p.Id == publicacion.Id);
            if (existente == null)
            {
                return HttpNotFound();
            }

            if (!usuario.EsAdministrador && existente.AutorId != usuario.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                existente.Titulo = publicacion.Titulo;
                existente.Contenido = publicacion.Contenido;
                existente.CategoriaId = publicacion.CategoriaId;
                _db.SaveChanges();
                return RedirectToAction("Details", new { id = existente.Id });
            }

            ViewBag.CategoriaId = new SelectList(_db.Categorias.OrderBy(c => c.Nombre), "Id", "Nombre", publicacion.CategoriaId);
            return View(publicacion);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var publicacion = _db.Publicaciones
                .Include(p => p.Autor)
                .Include(p => p.Categoria)
                .SingleOrDefault(p => p.Id == id.Value);

            if (publicacion == null)
            {
                return HttpNotFound();
            }

            var usuario = GetUsuarioActual();
            if (usuario == null || (!usuario.EsAdministrador && publicacion.AutorId != usuario.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(publicacion);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var publicacion = _db.Publicaciones.Find(id);
            if (publicacion == null)
            {
                return HttpNotFound();
            }

            var usuario = GetUsuarioActual();
            if (usuario == null || (!usuario.EsAdministrador && publicacion.AutorId != usuario.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            _db.Publicaciones.Remove(publicacion);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        private Usuario GetUsuarioActual()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var email = User.Identity.Name;
            return _db.Usuarios.SingleOrDefault(u => u.Email == email);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
