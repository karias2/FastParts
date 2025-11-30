
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FastParts.Models
{
    public class AlertasInventarioVM
    {
        public List<RepuestoModel> BajoMinimo { get; set; }
        public List<AlertaInventarioModel> Alertas { get; set; }
    }
}