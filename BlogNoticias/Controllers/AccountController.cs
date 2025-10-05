using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using BlogNoticias.DAL;
using BlogNoticias.Models;
using BlogNoticias.Models.ViewModels;

namespace BlogNoticias.Controllers
{
    public class AccountController : Controller
    {
        private readonly BlogContext _db = new BlogContext();

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emailExiste = _db.Usuarios.Any(u => u.Email == model.Email);
            if (emailExiste)
            {
                ModelState.AddModelError("Email", "El correo ya está registrado.");
                return View(model);
            }

            var usuario = new Usuario
            {
                NombreCompleto = model.NombreCompleto,
                Email = model.Email,
                PasswordHash = Crypto.HashPassword(model.Password),
                FechaRegistro = DateTime.UtcNow,
                EsAdministrador = false
            };

            _db.Usuarios.Add(usuario);
            _db.SaveChanges();

            SignInUser(usuario, true);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = _db.Usuarios.SingleOrDefault(u => u.Email == model.Email);
            if (usuario == null || !Crypto.VerifyHashedPassword(usuario.PasswordHash, model.Password))
            {
                ModelState.AddModelError("", "Credenciales inválidas.");
                return View(model);
            }

            SignInUser(usuario, model.RememberMe);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty)
            {
                Expires = DateTime.UtcNow.AddDays(-1)
            };
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }

        private void SignInUser(Usuario usuario, bool rememberMe)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                usuario.Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                rememberMe,
                usuario.EsAdministrador.ToString());

            var encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
            {
                HttpOnly = true
            };

            if (rememberMe)
            {
                cookie.Expires = ticket.Expiration;
            }

            Response.Cookies.Add(cookie);
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
