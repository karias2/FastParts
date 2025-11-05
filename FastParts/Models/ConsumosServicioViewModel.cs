
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FastParts.Models
{
    public class ConsumosServicioVM
    {
        public int ServicioId { get; set; }
        public IEnumerable<ServicioRepuestoModel> Consumos { get; set; }
    }
}