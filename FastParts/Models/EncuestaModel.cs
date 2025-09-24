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

        public virtual ICollection<PreguntaModel> Preguntas { get; set; }
    }

    public class EncuestaViewModel
    {
        public int ID_Encuesta { get; set; }

        public virtual EncuestaModel Encuesta { get; set; }
        public virtual PreguntaModel Pregunta { get; set; }

        public List<System.Web.Mvc.SelectListItem> TiposDePregunta { get; set; }
    }
}