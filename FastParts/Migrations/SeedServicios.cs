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
    public class SeedServicios
    {

        public void AddServicios(FastParts.Models.ApplicationDbContext context)
        {

            context.ServicioModels.AddOrUpdate(
                s => s.Nombre,
                new ServicioModel
                {
                    Nombre = "Cambio de aceite",
                    Descripcion = "Servicio básico de cambio de aceite y revisión general.",
                    PrecioServicio = 15000,
                    Activo = true
                },
                 new ServicioModel
                 {
                     Nombre = "Alineación y balanceo",
                     Descripcion = "Ajuste de la alineación y balanceo de las llantas.",
                     PrecioServicio = 25000,
                     Activo = true
                 },

                new ServicioModel
                {
                    Nombre = "Revisión y cambio de frenos",
                    Descripcion = "Inspección del sistema de frenos y reemplazo de pastillas o discos si es necesario.",
                    PrecioServicio = 40000,
                    Activo = true
                },

                new ServicioModel
                {
                    Nombre = "Diagnóstico computarizado",
                    Descripcion = "Análisis electrónico del vehículo para detectar fallas mediante escáner especializado.",
                    PrecioServicio = 20000,
                    Activo = true
                },

                new ServicioModel
                {
                    Nombre = "Cambio de batería",
                    Descripcion = "Revisión del sistema eléctrico y reemplazo de batería del vehículo.",
                    PrecioServicio = 18000,
                    Activo = true
                },

                new ServicioModel
                {
                    Nombre = "Revisión de suspensión",
                    Descripcion = "Inspección de amortiguadores, resortes y componentes de suspensión.",
                    PrecioServicio = 30000,
                    Activo = true
                },

                new ServicioModel
                {
                    Nombre = "Mantenimiento preventivo completo",
                    Descripcion = "Servicio integral que incluye revisión de fluidos, frenos, suspensión y sistema eléctrico.",
                    PrecioServicio = 60000,
                    Activo = true
                },

                new ServicioModel
                {
                    Nombre = "Reparación de sistema de enfriamiento",
                    Descripcion = "Diagnóstico y reparación de radiador, mangueras y sistema de refrigeración.",
                    PrecioServicio = 45000,
                    Activo = true
                },

                new ServicioModel
                {
                    Nombre = "Escaneo y borrado de códigos de falla",
                    Descripcion = "Lectura y eliminación de códigos de error del sistema electrónico del vehículo.",
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
        }
    }
}