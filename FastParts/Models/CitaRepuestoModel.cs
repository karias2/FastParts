using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class CitaRepuestoModel
    {
        public int Id { get; set; }
        public int CitaId { get; set; }
        public int RepuestoId { get; set; }
        public int Cantidad { get; set; }

        [ForeignKey("CitaId")]
        public virtual CitaModel Cita { get; set; }

        [ForeignKey("RepuestoId")]
        public virtual RepuestoModel Repuesto { get; set; }
    }
}