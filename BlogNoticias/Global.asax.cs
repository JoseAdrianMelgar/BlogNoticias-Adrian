using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Claims;
using System.Web;
using System.Web.Security;

namespace BlogNoticias
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(object sender, System.EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return;
            }

            try
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || ticket.Expired)
                {
                    return;
                }

                var identity = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                identity.AddClaim(new Claim(ClaimTypes.Name, ticket.Name));
                identity.AddClaim(new Claim("EsAdministrador", ticket.UserData));

                var principal = new ClaimsPrincipal(identity);
                HttpContext.Current.User = principal;
                System.Threading.Thread.CurrentPrincipal = principal;
            }
            catch
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}
