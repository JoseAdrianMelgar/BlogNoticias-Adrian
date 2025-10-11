using System.Data.Entity;
using BlogNoticias.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogNoticias.DAL
{
    public class BlogContext : IdentityDbContext<Usuario, Rol, int, UsuarioLogin, UsuarioRole, UsuarioClaim>
    {
        public BlogContext() : base("DefaultConnection")
        {
        }

        public static BlogContext Create()
        {
            return new BlogContext();
        }

        public IDbSet<Usuario> Usuarios => Users;

        public DbSet<Publicacion> Publicaciones { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Rol>().ToTable("Roles");
            modelBuilder.Entity<UsuarioRole>().ToTable("UsuarioRoles");
            modelBuilder.Entity<UsuarioLogin>().ToTable("UsuarioLogins");
            modelBuilder.Entity<UsuarioClaim>().ToTable("UsuarioClaims");

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Publicaciones)
                .WithRequired(p => p.Autor)
                .HasForeignKey(p => p.AutorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Comentarios)
                .WithRequired(c => c.Autor)
                .HasForeignKey(c => c.AutorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Publicacion>()
                .HasMany(p => p.Comentarios)
                .WithRequired(c => c.Publicacion)
                .HasForeignKey(c => c.PublicacionId);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.NombreCompleto)
                .IsRequired()
                .HasMaxLength(120);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasMaxLength(256);
        }
    }
}
