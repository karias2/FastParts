using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FastParts.Models
{

    public class Recordatorio
    {
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Fecha del Servicio")]
        [DataType(DataType.Date)]
        public DateTime FechaServicio { get; set; }

        [Display(Name = "Métodos de Envío")]
        public List<string> MetodosEnvio { get; set; } = new List<string>();

        public bool Activo { get; set; } = true;
    }
}