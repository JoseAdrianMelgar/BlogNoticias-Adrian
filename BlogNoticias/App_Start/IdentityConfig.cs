using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogNoticias.DAL;
using BlogNoticias.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogNoticias
{
    public class UsuarioStore : UserStore<Usuario, Rol, int, UsuarioLogin, UsuarioRole, UsuarioClaim>
    {
        public UsuarioStore(BlogContext context) : base(context)
        {
        }
    }

    public class RolStore : RoleStore<Rol, int, UsuarioRole>
    {
        public RolStore(BlogContext context) : base(context)
        {
        }
    }

    public class ApplicationUserManager : UserManager<Usuario, int>
    {
        public ApplicationUserManager(IUserStore<Usuario, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UsuarioStore(context.Get<BlogContext>()));

            manager.UserValidator = new UserValidator<Usuario, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonLetterOrDigit = true
            };

            manager.UserLockoutEnabledByDefault = false;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Usuario, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<Usuario, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(Usuario user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
