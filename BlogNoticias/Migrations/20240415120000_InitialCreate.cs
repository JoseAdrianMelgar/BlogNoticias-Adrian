using System.Data.Entity.Migrations;

namespace BlogNoticias.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreCompleto = c.String(nullable: false, maxLength: 120),
                        Email = c.String(nullable: false, maxLength: 120),
                        PasswordHash = c.String(nullable: false, maxLength: 255),
                        FechaRegistro = c.DateTime(nullable: false),
                        EsAdministrador = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "IX_Usuario_Email");

            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nombre, unique: true, name: "IX_Categoria_Nombre");

            CreateTable(
                "dbo.Publicaciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Contenido = c.String(nullable: false),
                        FechaPublicacion = c.DateTime(nullable: false),
                        AutorId = c.Int(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.AutorId)
                .ForeignKey("dbo.Categorias", t => t.CategoriaId, cascadeDelete: true)
                .Index(t => t.CategoriaId)
                .Index(t => t.AutorId);

            CreateTable(
                "dbo.Comentarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contenido = c.String(nullable: false),
                        FechaComentario = c.DateTime(nullable: false),
                        AutorId = c.Int(nullable: false),
                        PublicacionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.AutorId)
                .ForeignKey("dbo.Publicaciones", t => t.PublicacionId, cascadeDelete: true)
                .Index(t => t.PublicacionId)
                .Index(t => t.AutorId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Comentarios", "PublicacionId", "dbo.Publicaciones");
            DropForeignKey("dbo.Comentarios", "AutorId", "dbo.Usuarios");
            DropForeignKey("dbo.Publicaciones", "CategoriaId", "dbo.Categorias");
            DropForeignKey("dbo.Publicaciones", "AutorId", "dbo.Usuarios");
            DropIndex("dbo.Comentarios", new[] { "AutorId" });
            DropIndex("dbo.Comentarios", new[] { "PublicacionId" });
            DropIndex("dbo.Publicaciones", new[] { "AutorId" });
            DropIndex("dbo.Publicaciones", new[] { "CategoriaId" });
            DropIndex("dbo.Categorias", "IX_Categoria_Nombre");
            DropIndex("dbo.Usuarios", "IX_Usuario_Email");
            DropTable("dbo.Comentarios");
            DropTable("dbo.Publicaciones");
            DropTable("dbo.Categorias");
            DropTable("dbo.Usuarios");
        }
    }
}
