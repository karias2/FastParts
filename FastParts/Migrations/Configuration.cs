namespace FastParts.Migrations
{
    using FastParts.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FastParts.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FastParts.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            if (!context.Repuestos.Any())
            {
                context.Repuestos.Add(new RepuestoModel { Nombre = "Filtro de Aceite", Marca = "ACME", NumeroParte = "FA-100", Precio = 4500, Stock = 20, Proveedor = "Proveedor 1" });
                context.Repuestos.Add(new RepuestoModel { Nombre = "Bujía", Marca = "SparkX", NumeroParte = "BX-200", Precio = 2500, Stock = 100, Proveedor = "Proveedor 2" });
                context.SaveChanges();
            }

            if (!context.Encuestas.Any())
            {
                var EncuestaSatisfaccion = new EncuestaModel
                {
                    Nombre = "Encuesta de Satisfacción",
                    Descripcion = "Encuesta para medir el nivel de satisfacción del cliente con el servicio de FastParts.",
                    Preguntas = new List<PreguntaModel>()
                };

                context.Encuestas.Add(EncuestaSatisfaccion);
                context.SaveChanges();


                context.Preguntas.Add(new PreguntaModel
                {
                    ID_Encuesta = EncuestaSatisfaccion.ID_Encuesta,
                    Descripcion = "¿Cómo califica la calidad del servicio?",
                    Tipo = "Rango",
                    Minimo = 1,
                    Maximo = 5
                });

                context.Preguntas.Add(new PreguntaModel
                {
                    ID_Encuesta = EncuestaSatisfaccion.ID_Encuesta,
                    Descripcion = "¿Cómo califica el tiempo de entrega?",
                    Tipo = "Rango",
                    Minimo = 1,
                    Maximo = 5
                });

                context.Preguntas.Add(new PreguntaModel
                {
                    ID_Encuesta = EncuestaSatisfaccion.ID_Encuesta,
                    Descripcion = "¿Cómo califica el trato recibido?",
                    Tipo = "Rango",
                    Minimo = 1,
                    Maximo = 5
                });

                context.Preguntas.Add(new PreguntaModel
                {
                    ID_Encuesta = EncuestaSatisfaccion.ID_Encuesta,
                    Descripcion = "Comentarios adicionales (opcional)",
                    Tipo = "Texto"
                });

                context.SaveChanges();
            }
        }
    }
}
