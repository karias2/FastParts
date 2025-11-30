
using System;

namespace FastParts.Models
{
    public enum TipoMovimiento { Entrada, Salida, Ajuste }

    public class MovimientoInventarioModel
    {
        public int Id { get; set; }
        public int RepuestoId { get; set; }
        public TipoMovimiento Tipo { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string UsuarioId { get; set; } 
        public int? ServicioRepuestoId { get; set; } 

        public virtual RepuestoModel Repuesto { get; set; }
        public virtual ServicioRepuestoModel ServicioRepuesto { get; set; }
    }
}