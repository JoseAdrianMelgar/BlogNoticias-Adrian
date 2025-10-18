using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using SistemaInventario.Data;
using SistemaInventario.Models;

namespace SistemaInventario.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var proveedores = new List<Proveedor>
            {
                new Proveedor { Nombre = "Distribuidora Central", Telefono = "555-1010", Email = "contacto@central.com", Direccion = "Zona 1, Ciudad", Activo = true },
                new Proveedor { Nombre = "Tecnología Global", Telefono = "555-2020", Email = "ventas@tecnologiaglobal.com", Direccion = "Zona 9, Ciudad", Activo = true },
                new Proveedor { Nombre = "OfiMax", Telefono = "555-3030", Email = "info@ofimax.com", Direccion = "Zona 10, Ciudad", Activo = true },
                new Proveedor { Nombre = "PaperExpress", Telefono = "555-4040", Email = "pedidos@paperexpress.com", Direccion = "Zona 4, Ciudad", Activo = true },
                new Proveedor { Nombre = "Suministros Médicos", Telefono = "555-5050", Email = "ventas@sumed.com", Direccion = "Zona 12, Ciudad", Activo = true }
            };

            foreach (var proveedor in proveedores)
            {
                context.Proveedores.AddOrUpdate(p => p.Nombre, proveedor);
            }

            context.SaveChanges();

            var proveedoresDb = context.Proveedores.ToList();
            var proveedorPorNombre = proveedoresDb.ToDictionary(p => p.Nombre, p => p.ProveedorId);

            var productos = new List<Producto>
            {
                new Producto { Nombre = "Laptop Empresarial", Descripcion = "Laptop 15\" Core i5", PrecioCosto = 650m, PrecioVenta = 820m, StockActual = 12, StockMinimo = 5, Activo = true, ProveedorId = proveedorPorNombre["Tecnología Global"] },
                new Producto { Nombre = "Impresora Láser", Descripcion = "Impresora monocromática", PrecioCosto = 180m, PrecioVenta = 260m, StockActual = 8, StockMinimo = 3, Activo = true, ProveedorId = proveedorPorNombre["Distribuidora Central"] },
                new Producto { Nombre = "Resma Papel Carta", Descripcion = "Paquete de 500 hojas", PrecioCosto = 3.5m, PrecioVenta = 6.5m, StockActual = 40, StockMinimo = 20, Activo = true, ProveedorId = proveedorPorNombre["PaperExpress"] },
                new Producto { Nombre = "Mouse Inalámbrico", Descripcion = "Mouse ergonómico", PrecioCosto = 12m, PrecioVenta = 25m, StockActual = 25, StockMinimo = 10, Activo = true, ProveedorId = proveedorPorNombre["Tecnología Global"] },
                new Producto { Nombre = "Teclado Mecánico", Descripcion = "Teclado retroiluminado", PrecioCosto = 45m, PrecioVenta = 70m, StockActual = 10, StockMinimo = 4, Activo = true, ProveedorId = proveedorPorNombre["Tecnología Global"] },
                new Producto { Nombre = "Silla Ergonómica", Descripcion = "Silla ajustable para oficina", PrecioCosto = 95m, PrecioVenta = 150m, StockActual = 14, StockMinimo = 5, Activo = true, ProveedorId = proveedorPorNombre["OfiMax"] },
                new Producto { Nombre = "Escritorio Modular", Descripcion = "Escritorio en L", PrecioCosto = 120m, PrecioVenta = 210m, StockActual = 6, StockMinimo = 2, Activo = true, ProveedorId = proveedorPorNombre["OfiMax"] },
                new Producto { Nombre = "Cartuchos Tinta", Descripcion = "Set de tintas CMYK", PrecioCosto = 28m, PrecioVenta = 45m, StockActual = 18, StockMinimo = 6, Activo = true, ProveedorId = proveedorPorNombre["Distribuidora Central"] },
                new Producto { Nombre = "Guantes Látex", Descripcion = "Caja con 100 unidades", PrecioCosto = 9m, PrecioVenta = 16m, StockActual = 30, StockMinimo = 10, Activo = true, ProveedorId = proveedorPorNombre["Suministros Médicos"] },
                new Producto { Nombre = "Alcohol en Gel", Descripcion = "Galón de 1L", PrecioCosto = 6m, PrecioVenta = 11m, StockActual = 22, StockMinimo = 8, Activo = true, ProveedorId = proveedorPorNombre["Suministros Médicos"] }
            };

            foreach (var producto in productos)
            {
                context.Productos.AddOrUpdate(p => p.Nombre, producto);
            }

            context.SaveChanges();

            var productosDb = context.Productos.ToList();
            var productoPorNombre = productosDb.ToDictionary(p => p.Nombre, p => p.ProductoId);

            var movimientos = new List<MovimientoInventario>
            {
                new MovimientoInventario { ProductoId = productoPorNombre["Laptop Empresarial"], Tipo = TipoMovimiento.Entrada, Cantidad = 5, Fecha = DateTime.Today.AddDays(-15), ProveedorId = proveedorPorNombre["Tecnología Global"], Observaciones = "Reposición inicial" },
                new MovimientoInventario { ProductoId = productoPorNombre["Laptop Empresarial"], Tipo = TipoMovimiento.Salida, Cantidad = 3, Fecha = DateTime.Today.AddDays(-10), Observaciones = "Venta a departamento IT" },
                new MovimientoInventario { ProductoId = productoPorNombre["Resma Papel Carta"], Tipo = TipoMovimiento.Entrada, Cantidad = 50, Fecha = DateTime.Today.AddDays(-12), ProveedorId = proveedorPorNombre["PaperExpress"], Observaciones = "Compra mensual" },
                new MovimientoInventario { ProductoId = productoPorNombre["Resma Papel Carta"], Tipo = TipoMovimiento.Salida, Cantidad = 30, Fecha = DateTime.Today.AddDays(-5), Observaciones = "Consumo oficinas" },
                new MovimientoInventario { ProductoId = productoPorNombre["Mouse Inalámbrico"], Tipo = TipoMovimiento.Salida, Cantidad = 8, Fecha = DateTime.Today.AddDays(-7), Observaciones = "Asignación personal nuevo" },
                new MovimientoInventario { ProductoId = productoPorNombre["Cartuchos Tinta"], Tipo = TipoMovimiento.Entrada, Cantidad = 12, Fecha = DateTime.Today.AddDays(-9), ProveedorId = proveedorPorNombre["Distribuidora Central"], Observaciones = "Pedido trimestral" },
                new MovimientoInventario { ProductoId = productoPorNombre["Guantes Látex"], Tipo = TipoMovimiento.Salida, Cantidad = 10, Fecha = DateTime.Today.AddDays(-6), Observaciones = "Entrega a laboratorio" },
                new MovimientoInventario { ProductoId = productoPorNombre["Alcohol en Gel"], Tipo = TipoMovimiento.Entrada, Cantidad = 20, Fecha = DateTime.Today.AddDays(-3), ProveedorId = proveedorPorNombre["Suministros Médicos"], Observaciones = "Refuerzo de inventario" }
            };

            foreach (var movimiento in movimientos)
            {
                bool existe = context.MovimientosInventario.Any(m =>
                    m.ProductoId == movimiento.ProductoId &&
                    DbFunctions.TruncateTime(m.Fecha) == movimiento.Fecha.Date &&
                    m.Cantidad == movimiento.Cantidad &&
                    m.Tipo == movimiento.Tipo);

                if (!existe)
                {
                    context.MovimientosInventario.Add(movimiento);

                    var producto = context.Productos.Single(p => p.ProductoId == movimiento.ProductoId);
                    if (movimiento.Tipo == TipoMovimiento.Entrada)
                    {
                        producto.StockActual += movimiento.Cantidad;
                    }
                    else if (producto.StockActual - movimiento.Cantidad >= 0)
                    {
                        producto.StockActual -= movimiento.Cantidad;
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
