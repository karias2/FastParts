using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace FastParts.Models
{
    public class RepuestoModel
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Stock mínimo")]
        public int StockMinimo { get; set; } = 0;

        [Display(Name = "Ocultar a clientes")]
        public bool OcultarClientes { get; set; } = false;

        [Display(Name = "Forzar sin stock")]
        public bool SinStockForzado { get; set; } = false;

        [NotMapped]
        public bool EstaBajoMinimo => Stock <= StockMinimo;

        [NotMapped]
        public bool MostrarEnCatalogo => !OcultarClientes && !SinStockForzado && Stock > 0;

        [StringLength(80)]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [StringLength(80)]
        [Display(Name = "Número de Parte")]
        public string NumeroParte { get; set; }

        [Range(0, 999999)]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [StringLength(200)]
        [Display(Name = "Proveedor")]
        public string Proveedor { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(300)]
        public string ImagenUrl { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImagenFile { get; set; }

        // --- Soft delete ---
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
