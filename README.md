# BlogNoticias-Adrian

Este repositorio contiene la solución **BlogNoticias**, un proyecto web ASP.NET MVC 5 (.NET Framework) con Entity Framework 6 y autenticación por formularios. El proyecto incluye:

- Archivo de solución `BlogNoticias.sln`.
- Proyecto MVC `BlogNoticias/BlogNoticias.csproj` con configuración de conexión a `(localdb)\MSSQLLocalDB` en `Web.config`.
- Carpetas requeridas: `Models`, `Controllers`, `Views`, `DAL`, `Migrations`, `Content` y `Scripts`.
- Modelos `Usuario`, `Publicacion`, `Categoria` y `Comentario` junto con el contexto `BlogContext`.
- Controladores y vistas para registro/inicio de sesión, CRUD de publicaciones con categorías y comentarios, además de un panel de administración.

El objetivo es proporcionar un punto de partida completo para la plataforma BlogNoticias.
