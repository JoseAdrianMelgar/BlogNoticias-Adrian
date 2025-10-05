using System.Data.Entity;
using BlogNoticias.Models;

namespace BlogNoticias.DAL
{
    public class BlogContext : DbContext
    {
        public BlogContext() : base("BlogContext")
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Publicacion> Publicaciones { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
}
