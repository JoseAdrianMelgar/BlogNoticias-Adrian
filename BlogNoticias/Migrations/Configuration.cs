using System.Data.Entity.Migrations;
using BlogNoticias.DAL;
using BlogNoticias.Models;
using System;
using System.Linq;
using System.Web.Helpers;

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
                var admin = new Usuario
                {
                    NombreCompleto = "Administrador",
                    Email = "admin@blognoticias.com",
                    PasswordHash = Crypto.HashPassword("Admin123$"),
                    FechaRegistro = DateTime.UtcNow,
                    EsAdministrador = true
                };
                context.Usuarios.Add(admin);
            }
        }
    }
}
