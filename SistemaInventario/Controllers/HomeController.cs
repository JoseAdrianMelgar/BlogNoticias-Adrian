using System.Web.Mvc;

namespace SistemaInventario.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Sistema de Inventario";
            return View();
        }

        public ActionResult Acerca()
        {
            return View();
        }
    }
}
