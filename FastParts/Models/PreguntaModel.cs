using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class PreguntaModel
    {
        [Key]
        public int ID_Pregunta { get; set; }

        public int ID_Encuesta { get; set; }
        [ForeignKey("ID_Encuesta")]
        public virtual EncuestaModel Encuesta { get; set; }

        [Required]
        [StringLength(400)]
        public string Descripcion { get; set; }

        [Required]
        public TipoPregunta Tipo { get; set; }

        // Separados por ,
        public string opciones { get; set; }

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
        Texto,

        //
        OpcionMultiple,

        //
        CasillasVerificacion
    }
}