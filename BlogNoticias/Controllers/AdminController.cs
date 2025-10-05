using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogNoticias.DAL;
using BlogNoticias.Filters;

namespace BlogNoticias.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly BlogContext _db = new BlogContext();

        public ActionResult Index()
        {
            ViewBag.TotalUsuarios = _db.Usuarios.Count();
            ViewBag.TotalPublicaciones = _db.Publicaciones.Count();
            ViewBag.TotalComentarios = _db.Comentarios.Count();
            return View();
        }

        public ActionResult Usuarios()
        {
            var usuarios = _db.Usuarios.OrderBy(u => u.NombreCompleto).ToList();
            return View(usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlternarAdministrador(int id)
        {
            var usuario = _db.Usuarios.SingleOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            usuario.EsAdministrador = !usuario.EsAdministrador;
            _db.SaveChanges();
            return RedirectToAction("Usuarios");
        }

        public ActionResult Publicaciones()
        {
            var publicaciones = _db.Publicaciones
                .Include(p => p.Autor)
                .Include(p => p.Categoria)
                .OrderByDescending(p => p.FechaPublicacion)
                .ToList();
            return View(publicaciones);
        }

        public ActionResult Comentarios()
        {
            var comentarios = _db.Comentarios
                .Include(c => c.Autor)
                .Include(c => c.Publicacion)
                .OrderByDescending(c => c.FechaComentario)
                .ToList();
            return View(comentarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarComentario(int id)
        {
            var comentario = _db.Comentarios.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }

            _db.Comentarios.Remove(comentario);
            _db.SaveChanges();
            return RedirectToAction("Comentarios");
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
