using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastParts.Migrations
{
    public class SeedEncuestas
    {
        public void Satisfaccion(FastParts.Models.ApplicationDbContext context)
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
                Tipo = "CasillasVerificacion",
                Opciones = "Rapido, Lento"
            });

            context.Preguntas.Add(new PreguntaModel
            {
                ID_Encuesta = EncuestaSatisfaccion.ID_Encuesta,
                Descripcion = "¿Cómo califica el trato recibido?",
                Tipo = "OpcionMultiple",
                Opciones = "Bueno, Malo, Se Puede Mejorar"
            });

            context.Preguntas.Add(new PreguntaModel
            {
                ID_Encuesta = EncuestaSatisfaccion.ID_Encuesta,
                Descripcion = "Comentarios adicionales (opcional)",
                Tipo = "Texto"
            });

            context.Preguntas.Add(new PreguntaModel
            {
                ID_Encuesta = EncuestaSatisfaccion.ID_Encuesta,
                Descripcion = "Eviencias",
                Tipo = "Imagen"
            });

            context.SaveChanges();
        }
    }
}