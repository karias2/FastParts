using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class EncuestaModel
    {
        [Key]
        public int ID_Encuesta { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //[Required]
        //[ForeignKey("Cita")]
        //public int ID_Cita { get; set; }

        // Propiedades de navegación para las relaciones

        //[ForeignKey("ID_Cita")]
        //public virtual Cita Cita { get; set; }

        public virtual ICollection<PreguntaModel> Preguntas { get; set; }
    }

    public class EncuestaViewModel
    {
        // borrar

        public DateTime Fecha { get; set; }
        public int IdPedido { get; set; }  // opcional, para asociar al servicio recibido

        [Range(1, 5)]
        public int CalificacionServicio { get; set; }


        [Range(1, 5)]
        public int CalificacionTiempo { get; set; }

        [Range(1, 5)]
        public int CalificacionTrato { get; set; }

        public string ComentariosAdicionales { get; set; }
    }
}