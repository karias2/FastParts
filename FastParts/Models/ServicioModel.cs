using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class ServicioModel
    {
        [Key]
        public int IdServicio { get; set; }

        [Required]
        [Display(Name = "Nombre de servicio")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Descripcion de servicio")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Precio del servicio")]
        public decimal PrecioServicio{ get; set; }

        public bool Activo { get; set; } = true;
    }
}