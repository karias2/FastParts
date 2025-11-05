
using System.ComponentModel.DataAnnotations;

namespace FastParts.Models
{
    public class ServicioRepuestoModel
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public int RepuestoId { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }

        public virtual ServicioModel Servicio { get; set; }
        public virtual RepuestoModel Repuesto { get; set; }
    }
}