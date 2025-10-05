using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BlogNoticias.DAL;

namespace BlogNoticias.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogContext _db = new BlogContext();

        public ActionResult Index()
        {
            var publicaciones = _db.Publicaciones
                .Include(p => p.Autor)
                .Include(p => p.Categoria)
                .OrderByDescending(p => p.FechaPublicacion)
                .Take(6)
                .ToList();
            return View(publicaciones);
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
