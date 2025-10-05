using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogNoticias.DAL;

namespace BlogNoticias.Filters
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext))
            {
                return false;
            }

            var email = httpContext.User.Identity.Name;
            using (var db = new BlogContext())
            {
                var usuario = db.Usuarios.SingleOrDefault(u => u.Email == email);
                return usuario != null && usuario.EsAdministrador;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(403);
            }
        }
    }
}
