namespace FastParts.Migrations
{
    using FastParts.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<FastParts.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FastParts.Models.ApplicationDbContext context)
        {
            // Repuestos (idempotente por clave compuesta Nombre+NumeroParte)
            context.Repuestos.AddOrUpdate(
                r => new { r.Nombre, r.NumeroParte },
                new RepuestoModel
                {
                    Nombre = "Filtro de Aceite",
                    Marca = "ACME",
                    NumeroParte = "FA-100",
                    Precio = 4500,
                    Stock = 5,
                    StockMinimo = 3,
                    Proveedor = "Proveedor 1",
                    OcultarClientes = false,
                    SinStockForzado = false
                },
                new RepuestoModel
                {
                    Nombre = "Bujía",
                    Marca = "SparkX",
                    NumeroParte = "BX-200",
                    Precio = 2500,
                    Stock = 2,
                    StockMinimo = 2,
                    Proveedor = "Proveedor 2",
                    OcultarClientes = false,
                    SinStockForzado = false
                },
                new RepuestoModel
                {
                    Nombre = "Filtro de Combustible",
                    Marca = "ACME",
                    NumeroParte = "FC-150",
                    Precio = 5200,
                    Stock = 2,
                    StockMinimo = 1,
                    Proveedor = "Proveedor 1",
                    OcultarClientes = false,
                    SinStockForzado = false
                },
                new RepuestoModel
                {
                    Nombre = "Carburador",
                    Marca = "SparkX",
                    NumeroParte = "CB-300",
                    Precio = 25000,
                    Stock = 0,
                    StockMinimo = 1,
                    Proveedor = "Proveedor 2",
                    OcultarClientes = true, 
                    SinStockForzado = true
                }
            );

            // Servicio base
            context.ServicioModels.AddOrUpdate(
    s => s.Nombre,
    new ServicioModel
    {
        Nombre = "Cambio de aceite",
        Descripcion = "Servicio básico de cambio de aceite y revisión general.",
        PrecioServicio = 15000,
        Activo = true
    }
);


            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    sb.AppendLine($"Entidad: {eve.Entry.Entity.GetType().Name}, Estado: {eve.Entry.State}");
                    foreach (var ve in eve.ValidationErrors)
                        sb.AppendLine($" - Propiedad: {ve.PropertyName} => {ve.ErrorMessage}");
                }

                Debug.WriteLine(sb.ToString());        
                throw new Exception("Errores de validación en Seed:\n" + sb, ex); 
            }

            if (!context.Encuestas.Any())
            {
                var SeedEncuenta = new SeedEncuestas();
                SeedEncuenta.Satisfaccion(context);
            }

            if (!roleManager.RoleExists("Mecanico"))
            {
                var SeedUsuarios = new SeedUsuarios();
                SeedUsuarios.CrearRolesYUsuarios(context);
            }
        }
    }
}
