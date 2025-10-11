using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogNoticias.DAL;
using Microsoft.AspNet.Identity;

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

            var userId = httpContext.User.Identity.GetUserId<int>();
            if (userId <= 0)
            {
                return false;
            }

            using (var db = new BlogContext())
            {
                var usuario = db.Usuarios.SingleOrDefault(u => u.Id == userId);
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
