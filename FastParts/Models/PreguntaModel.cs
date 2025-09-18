using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class PreguntaModel
    {
        [Key]
        public int ID_Pregunta { get; set; }

        [Required]
        [StringLength(400)]
        public string Descripcion { get; set; }

        [Required]
        public TipoPregunta Tipo { get; set; }

        // Propiedades opcionales para preguntas de tipo Rango
        public int? Minimo { get; set; }
        public int? Maximo { get; set; }
    }

    public enum TipoPregunta
    {
        // Pregunta de selección, como un rango de calificación.
        // Por ejemplo: ¿Qué tan satisfecho está con el servicio? (1 al 5)
        Rango,

        // Pregunta de texto libre, como un comentario.
        // Por ejemplo: Describa su experiencia.
        Texto
    }
}