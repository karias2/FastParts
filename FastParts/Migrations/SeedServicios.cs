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