using System;
using System.ComponentModel.DataAnnotations;

namespace FastParts.Models
{
    public class CotizacionListadoViewModel
    {
 public int IdCotizacion { get; set; }
        public string IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string EmailCliente { get; set; }
        public DateTime FechaCreacion { get; set; }
 public DateTime FechaCita { get; set; }
  public string Estado { get; set; }
     public int CantidadRepuestos { get; set; }
        public int CantidadServicios { get; set; }
    [DataType(DataType.Currency)]
        public decimal MontoTotal { get; set; }
    }
}
