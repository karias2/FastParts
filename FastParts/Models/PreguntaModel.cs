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
        public string Tipo { get; set; }

        public Boolean Requerido { get; set; }

        public Boolean Activa { get; set; } = true;

        // Separados por ,
        public string Opciones { get; set; }

        // Propiedades opcionales para preguntas de tipo Rango
        public int? Minimo { get; set; } = 0;
        public int? Maximo { get; set; } = 10;

        // Respuestas

        // .Include(p => p.Respuestas)
        public ICollection<RespuestasModel> Respuestas { get; set; } = new List<RespuestasModel>();

        public int? ValorRespuesta { get; set; }

        public string TextoRespuesta { get; set; }

    }

    public class RespuestasModel
    {
        [Key]
        public int ID_Respuesta { get; set; }
        public int ID_Encuesta { get; set; }
        public int ID_Pregunta { get; set; }
        [ForeignKey("ID_Pregunta")]
        public virtual PreguntaModel Pregunta { get; set; }
        public string Session_Id { get; set; }

        [Required]
        public string Tipo { get; set; }

        // Respuestas

        public int? ValorRespuesta { get; set; }

        public string TextoRespuesta { get; set; }
    }
}