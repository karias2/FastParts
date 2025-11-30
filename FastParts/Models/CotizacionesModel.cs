using FastParts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class CotizacionesModel
    {
        [Key]
        public int IdCotizacion { get; set; }

        [Display(Name = "Cliente")]
        public string IdCliente { get; set; }

        [Display(Name = "Responsable")]
        public string IdResponsable { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaCita { get; set; }

        public string Estado { get; set; } // ACTIVA, INACTIVA, REVICION, COLA, COMPLETADA

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
            this.FechaCreacion = DateTime.Now;
            this.FechaCita = DateTime.Now;
        }
    }
}

public class RepuestosCotizadosModel
{
    [Key]
    public int ID { get; set; }
    public int IdCotizacion { get; set; }
    [ForeignKey("IdCotizacion")]
    public virtual CotizacionesModel Cotizacion { get; set; }

    public int IdRepuesto { get; set; }
    [ForeignKey("IdRepuesto")]
    public virtual RepuestoModel Repuesto { get; set; }
}

public class ServiciosCotizadosModel
{
    [Key]
    public int ID { get; set; }
    [Display(Name = "Cliente")]
    public int IdCliente { get; set; }
    public int IdCotizacion { get; set; }
    [ForeignKey("IdCotizacion")]
    public virtual CotizacionesModel Cotizacion { get; set; }

    public int IdServicio { get; set; }
    [ForeignKey("IdServicio")]
    public virtual ServicioModel Repuesto { get; set; }
}