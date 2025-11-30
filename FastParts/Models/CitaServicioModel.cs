using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class CitaServicioModel
    {
        public int Id { get; set; }
        public int CitaId { get; set; }
        public int ServicioId { get; set; }
        public int Cantidad { get; set; }

        [ForeignKey("CitaId")]
        public virtual CitaModel Cita { get; set; }

        [ForeignKey("ServicioId")]
        public virtual ServicioModel Servicio { get; set; }
    }
}