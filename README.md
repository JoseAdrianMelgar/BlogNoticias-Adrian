# SistemaInventario

Proyecto web ASP.NET MVC 5 (.NET Framework 4.8) para la gestión de inventario con Entity Framework 6 (Code First + Migrations).

## Requisitos
- Visual Studio Community 2022
- .NET Framework 4.8
- SQL Server LocalDB `(localdb)\MSSQLLocalDB`

## Estructura principal
- `SistemaInventario.sln` solución principal.
- `SistemaInventario/` proyecto MVC con carpetas `Models`, `Data`, `Controllers`, `ViewModels`, `Views`, `Migrations`, `Content` y `Scripts`.
- `ApplicationDbContext` configurado con la cadena de conexión `DefaultConnection` y migraciones automáticas.
- Semillas iniciales para proveedores, productos y movimientos en `Migrations/Configuration.cs`.

## Pasos para ejecutar
1. Abrir `SistemaInventario.sln` en Visual Studio 2022.
2. Restaurar paquetes NuGet al cargar la solución.
3. Configurar `SistemaInventario` como proyecto de inicio.
4. Ejecutar las migraciones desde la Package Manager Console:
   ```powershell
   Enable-Migrations
   Add-Migration InitialCreate
   Update-Database
   ```
5. Presionar **F5** o **Ctrl+F5** para compilar y ejecutar la aplicación.

El sitio utiliza Bootstrap 5, jQuery y Chart.js desde CDN, e incluye reportes dinámicos, CRUD de productos y proveedores, control de movimientos y reporte de stock bajo.
