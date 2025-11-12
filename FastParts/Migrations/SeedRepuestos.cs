using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace FastParts.Migrations
{
    public class SeedRepuestos
    {
        public void addRepuestos(FastParts.Models.ApplicationDbContext context)
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
        }
    }
}