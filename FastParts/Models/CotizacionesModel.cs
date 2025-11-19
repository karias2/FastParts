using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class CotizacionesModel
    {
        public int IdCotizacion { get; set; }

        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Responsable")]
        public int IdResponsable { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime FechaCita { get; set; }

        public string Estado { get; set; } // ACTIVA, INACTIVA, REVICION, ASIGANADA, COMPLETADA

        [DataType(DataType.Currency)]
        public decimal MontoTotal { get; set; }

        // 1. Lista de Repuestos
        [Display(Name = "Repuestos Cotizados")]
        public List<RepuestoModel> RepuestosCotizados { get; set; } = new List<RepuestoModel>();

        // 2. Lista de Servicios incluidos en la cotización
        [Display(Name = "Servicios Cotizados")]
        public List<ServicioModel> ServiciosCotizados { get; set; } = new List<ServicioModel>();

        public CotizacionesModel()
        {
            this.RepuestosCotizados = new List<RepuestoModel>();
            this.ServiciosCotizados = new List<ServicioModel>();
        }
    }
}