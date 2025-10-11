using System.Data.Entity.Migrations;

namespace BlogNoticias.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreCompleto = c.String(nullable: false, maxLength: 120),
                        FechaRegistro = c.DateTime(nullable: false),
                        EsAdministrador = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(maxLength: 256),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Email, unique: true, name: "IX_Usuario_Email");

            CreateTable(
                "dbo.UsuarioRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Usuarios", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.UsuarioClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.UsuarioLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Usuarios", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

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
            DropForeignKey("dbo.UsuarioLogins", "UserId", "dbo.Usuarios");
            DropForeignKey("dbo.UsuarioClaims", "UserId", "dbo.Usuarios");
            DropForeignKey("dbo.UsuarioRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UsuarioRoles", "UserId", "dbo.Usuarios");
            DropIndex("dbo.Comentarios", new[] { "AutorId" });
            DropIndex("dbo.Comentarios", new[] { "PublicacionId" });
            DropIndex("dbo.Publicaciones", new[] { "AutorId" });
            DropIndex("dbo.Publicaciones", new[] { "CategoriaId" });
            DropIndex("dbo.Categorias", "IX_Categoria_Nombre");
            DropIndex("dbo.UsuarioLogins", new[] { "UserId" });
            DropIndex("dbo.UsuarioClaims", new[] { "UserId" });
            DropIndex("dbo.UsuarioRoles", new[] { "RoleId" });
            DropIndex("dbo.UsuarioRoles", new[] { "UserId" });
            DropIndex("dbo.Usuarios", "IX_Usuario_Email");
            DropIndex("dbo.Usuarios", "UserNameIndex");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropTable("dbo.Comentarios");
            DropTable("dbo.Publicaciones");
            DropTable("dbo.Categorias");
            DropTable("dbo.UsuarioLogins");
            DropTable("dbo.UsuarioClaims");
            DropTable("dbo.UsuarioRoles");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Roles");
        }
    }
}
