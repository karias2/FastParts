using System.Collections.Generic;

namespace FastParts.Models
{
    public class CarritoItemRepuestoVM
    {
        public int IdRepuesto { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }

    public class CarritoItemServicioVM
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }

    public class MiCarritoViewModel
    {
        public int IdCotizacion { get; set; }
        public List<CarritoItemRepuestoVM> Repuestos { get; set; } = new List<CarritoItemRepuestoVM>();
        public List<CarritoItemServicioVM> Servicios { get; set; } = new List<CarritoItemServicioVM>();
        public decimal TotalRepuestos { get; set; }
        public decimal TotalServicios { get; set; }
        public decimal Total => TotalRepuestos + TotalServicios;
    }
}
