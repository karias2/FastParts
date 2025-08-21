using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class Cotizacion
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public string ServicioSolicitado { get; set; }
        public string Observaciones { get; set; }
        public decimal MontoEstimado { get; set; }
        public bool Aprobada { get; set; }
    }
}