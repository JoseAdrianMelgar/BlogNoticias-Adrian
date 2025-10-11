using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogNoticias.DAL;
using BlogNoticias.Models;
using Microsoft.AspNet.Identity;

namespace BlogNoticias.Controllers
{
    [Authorize]
    public class ComentariosController : Controller
    {
        private readonly BlogContext _db = new BlogContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(int publicacionId, string contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido))
            {
                TempData["ComentarioError"] = "El comentario no puede estar vacío.";
                return RedirectToAction("Details", "Publicaciones", new { id = publicacionId });
            }

            var usuario = GetUsuarioActual();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var publicacion = _db.Publicaciones.Find(publicacionId);
            if (publicacion == null)
            {
                return HttpNotFound();
            }

            var comentario = new Comentario
            {
                Contenido = contenido,
                AutorId = usuario.Id,
                PublicacionId = publicacionId,
                FechaComentario = DateTime.UtcNow
            };

            _db.Comentarios.Add(comentario);
            _db.SaveChanges();

            return RedirectToAction("Details", "Publicaciones", new { id = publicacionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var comentario = _db.Comentarios.Include(c => c.Publicacion).SingleOrDefault(c => c.Id == id);
            if (comentario == null)
            {
                return HttpNotFound();
            }

            var usuario = GetUsuarioActual();
            if (usuario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var esPropietario = comentario.AutorId == usuario.Id;
            var esAutorPublicacion = comentario.Publicacion.AutorId == usuario.Id;
            if (!usuario.EsAdministrador && !esPropietario && !esAutorPublicacion)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var publicacionId = comentario.PublicacionId;
            _db.Comentarios.Remove(comentario);
            _db.SaveChanges();
            return RedirectToAction("Details", "Publicaciones", new { id = publicacionId });
        }

        private Usuario GetUsuarioActual()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = User.Identity.GetUserId<int>();
            return _db.Usuarios.SingleOrDefault(u => u.Id == userId);
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
