using System.Data.Entity.Migrations;
using BlogNoticias.DAL;
using BlogNoticias;
using BlogNoticias.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;

namespace BlogNoticias.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BlogContext context)
        {
            if (!context.Categorias.Any())
            {
                context.Categorias.AddOrUpdate(
                    new Categoria { Nombre = "Tecnología", Descripcion = "Novedades tecnológicas" },
                    new Categoria { Nombre = "Economía", Descripcion = "Noticias financieras y de negocios" },
                    new Categoria { Nombre = "Cultura", Descripcion = "Arte, música y cine" }
                );
            }

            if (!context.Usuarios.Any())
            {
                var userManager = new ApplicationUserManager(new UsuarioStore(context));
                var admin = new Usuario
                {
                    UserName = "admin@blognoticias.com",
                    Email = "admin@blognoticias.com",
                    NombreCompleto = "Administrador",
                    FechaRegistro = DateTime.UtcNow,
                    EsAdministrador = true,
                    EmailConfirmed = true
                };

                userManager.Create(admin, "Admin123$");
            }
        }
    }
}
