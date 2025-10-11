using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogNoticias.Models
{
    public class Usuario : IdentityUser<int, UsuarioLogin, UsuarioRole, UsuarioClaim>
    {
        [Required]
        [StringLength(120)]
        public string NombreCompleto { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool EsAdministrador { get; set; }

        public virtual ICollection<Publicacion> Publicaciones { get; set; } = new List<Publicacion>();

        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Usuario, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim("NombreCompleto", NombreCompleto ?? Email));
            userIdentity.AddClaim(new Claim("EsAdministrador", EsAdministrador.ToString()));
            return userIdentity;
        }
    }

    public class Rol : IdentityRole<int, UsuarioRole>
    {
        public Rol()
        {
        }

        public Rol(string name) : base(name)
        {
        }
    }

    public class UsuarioLogin : IdentityUserLogin<int>
    {
    }

    public class UsuarioRole : IdentityUserRole<int>
    {
    }

    public class UsuarioClaim : IdentityUserClaim<int>
    {
    }
}
