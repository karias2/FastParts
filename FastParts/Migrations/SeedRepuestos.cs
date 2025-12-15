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

            context.Repuestos.AddOrUpdate(
                r => new { r.Nombre, r.NumeroParte },
                new RepuestoModel
                {
                    Nombre = "Bumper Delantero BMW X5",
                    Marca = "BMW",
                    NumeroParte = "BMW-0112845",
                    Precio = 300000,
                    Stock = 5,
                    StockMinimo = 2,
                    Proveedor = "Proveedor 1",
                    OcultarClientes = false,
                    SinStockForzado = false,
                    ImagenUrl = "/Content/uploads/repuestos/49b32568-942e-4cd7-a52c-f0564ff07407.jpg"
                },
                new RepuestoModel
                {
                    Nombre = "Pastillas de Freno Delanteras",
                    Marca = "Mercedes-Benz",
                    NumeroParte = "MBZ-0078932",
                    Precio = 45000,
                    Stock = 10,
                    StockMinimo = 2,
                    Proveedor = "Proveedor 3",
                    OcultarClientes = false,
                    SinStockForzado = false,
                    ImagenUrl = "/Content/uploads/repuestos/044b589d-396f-4503-81e2-c0f3d500d346.jpeg"
                },
                new RepuestoModel
                {
                    Nombre = "Radiador de Motor",
                    Marca = "Volkswagen",
                    NumeroParte = "VW-0092317",
                    Precio = 145000,
                    Stock = 2,
                    StockMinimo = 1,
                    Proveedor = "Proveedor 1",
                    OcultarClientes = false,
                    SinStockForzado = false,
                    ImagenUrl = "/Content/uploads/repuestos/f18e27d7-0fcb-4ba8-9bb3-298c29bdf765.png"
                },
                new RepuestoModel
                {
                    Nombre = "Sensor de Oxígeno (O2)",
                    Marca = "Volvo",
                    NumeroParte = "VOL-0035784",
                    Precio = 35000,
                    Stock = 4,
                    StockMinimo = 1,
                    Proveedor = "Proveedor 1",
                    OcultarClientes = false,
                    SinStockForzado = false,
                    ImagenUrl = "/Content/uploads/repuestos/41022bdc-e486-4c4c-ada6-b1e79b704c0b.jpg"
                },
                new RepuestoModel
                {
                    Nombre = "Filtro de Aceite",
                    Marca = "Audi",
                    NumeroParte = "AUD-0045129",
                    Precio = 15000,
                    Stock = 20,
                    StockMinimo = 3,
                    Proveedor = "Proveedor 2",
                    OcultarClientes = false,
                    SinStockForzado = false,
                    ImagenUrl = "/Content/uploads/repuestos/a593c608-3894-4f55-b985-0e60810c1314.jpg"
                },
                new RepuestoModel
                {
                    Nombre = "Espejo Retrovisor Izquierdo BMW X3",
                    Marca = "BMW",
                    NumeroParte = "BMW-0145628",
                    Precio = 215000,
                    Stock = 2,
                    StockMinimo = 1,
                    Proveedor = "Proveedor 3",
                    OcultarClientes = false,
                    SinStockForzado = false,
                    ImagenUrl = "/Content/uploads/repuestos/407f11c7-bc4a-474d-8ad7-6dfc87fd25c2.png"
                },
                new RepuestoModel
                {
                    Nombre = "Amortiguador Trasero Macan 2015",
                    Marca = "Porsche",
                    NumeroParte = "POR-0089146",
                    Precio = 150000,
                    Stock = 3,
                    StockMinimo = 1,
                    Proveedor = "Proveedor 2",
                    OcultarClientes = false,
                    SinStockForzado = false,
                    ImagenUrl = "/Content/uploads/repuestos/db0138c3-4845-43bd-83b2-da9b0613740b.jpg"
                }, new RepuestoModel
                {
                    Nombre = "Batería AGM 12V 70Ah",
                    Marca = "Bosch",
                    NumeroParte = "BOS-070AGM",
                    Precio = 95000,
                    Stock = 6,
                    StockMinimo = 2,
                    Proveedor = "Proveedor 2",
                    OcultarClientes = false,
                    SinStockForzado = false,
                    ImagenUrl = "/Content/uploads/repuestos/bateria-agm.jpg"
                },
new RepuestoModel
{
    Nombre = "Disco de Freno Delantero",
    Marca = "Toyota",
    NumeroParte = "TYT-DF34821",
    Precio = 78000,
    Stock = 8,
    StockMinimo = 2,
    Proveedor = "Proveedor 1",
    OcultarClientes = false,
    SinStockForzado = false,
    ImagenUrl = "/Content/uploads/repuestos/disco-freno.jpg"
},
new RepuestoModel
{
    Nombre = "Filtro de Aire Motor",
    Marca = "Hyundai",
    NumeroParte = "HYU-AF9012",
    Precio = 12000,
    Stock = 25,
    StockMinimo = 5,
    Proveedor = "Proveedor 3",
    OcultarClientes = false,
    SinStockForzado = false,
    ImagenUrl = "/Content/uploads/repuestos/filtro-aire.jpg"
},
new RepuestoModel
{
    Nombre = "Bomba de Agua",
    Marca = "Ford",
    NumeroParte = "FRD-WP5521",
    Precio = 68000,
    Stock = 4,
    StockMinimo = 1,
    Proveedor = "Proveedor 1",
    OcultarClientes = false,
    SinStockForzado = false,
    ImagenUrl = "/Content/uploads/repuestos/bomba-agua.jpg"
},
new RepuestoModel
{
    Nombre = "Kit de Embrague",
    Marca = "Nissan",
    NumeroParte = "NIS-CLUT9087",
    Precio = 185000,
    Stock = 2,
    StockMinimo = 1,
    Proveedor = "Proveedor 2",
    OcultarClientes = false,
    SinStockForzado = false,
    ImagenUrl = "/Content/uploads/repuestos/kit-embrague.jpg"
},
new RepuestoModel
{
    Nombre = "Bobina de Encendido",
    Marca = "Chevrolet",
    NumeroParte = "CHE-IGN3312",
    Precio = 42000,
    Stock = 7,
    StockMinimo = 2,
    Proveedor = "Proveedor 3",
    OcultarClientes = false,
    SinStockForzado = false,
    ImagenUrl = "/Content/uploads/repuestos/bobina-encendido.jpg"
},
new RepuestoModel
{
    Nombre = "Correa de Distribución",
    Marca = "Mazda",
    NumeroParte = "MAZ-TB7721",
    Precio = 56000,
    Stock = 5,
    StockMinimo = 2,
    Proveedor = "Proveedor 1",
    OcultarClientes = false,
    SinStockForzado = false,
    ImagenUrl = "/Content/uploads/repuestos/correa-distribucion.jpg"
},
new RepuestoModel
{
    Nombre = "Alternador",
    Marca = "Kia",
    NumeroParte = "KIA-ALT4490",
    Precio = 165000,
    Stock = 3,
    StockMinimo = 1,
    Proveedor = "Proveedor 2",
    OcultarClientes = false,
    SinStockForzado = false,
    ImagenUrl = "/Content/uploads/repuestos/alternador.jpg"
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