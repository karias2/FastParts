using System;

namespace FastParts.Models
{
    public class AlertaInventarioModel
    {
        public int Id { get; set; }
        public int RepuestoId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Mensaje { get; set; }
        public bool Atendida { get; set; } = false;

        public virtual RepuestoModel Repuesto { get; set; }
    }
}