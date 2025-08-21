using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class Servicio
    {
        public int Id { get; set; } 
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class Cliente
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
        public string Correo { get; set; }
    }

    public class Cotizacion
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public List<Servicio> Servicios { get; set; }
        public decimal Total
        {
            get
            {
                decimal total = 0;
                if (Servicios != null)
                    total = Servicios.Sum(s => s.Cantidad * s.PrecioUnitario);
                return total;
            }
        }
        public bool Enviada { get; set; }
    }
}