using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class CrearCitaClienteViewModel
    {
        [Required]
        [Display(Name = "Nombre del cliente")]
        public string NombreCliente { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string TelefonoCliente { get; set; }

        [Required]
        [Display(Name = "Vehículo")]
        public string Vehiculo { get; set; }

        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Required]
        [Display(Name = "Fecha y hora de la cita")]
        public DateTime FechaCita { get; set; }

        [Display(Name = "Motivo / Comentarios")]
        public string Motivo { get; set; }
    }

    public class CrearCitaAdminViewModel
    {
        [Required]
        [Display(Name = "Nombre del cliente")]
        public string NombreCliente { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string TelefonoCliente { get; set; }

        [Required]
        [Display(Name = "Vehículo")]
        public string Vehiculo { get; set; }

        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Required]
        [Display(Name = "Fecha y hora de la cita")]
        public DateTime FechaCita { get; set; }

        [Display(Name = "Motivo / Comentarios")]
        public string Motivo { get; set; }
    }

    public class CitaListadoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Cliente")]
        public string NombreCliente { get; set; }

        [Display(Name = "Teléfono")]
        public string TelefonoCliente { get; set; }

        public string Vehiculo { get; set; }
        public string Placa { get; set; }

        [Display(Name = "Fecha cita")]
        public DateTime FechaCita { get; set; }

        public string Estado { get; set; }

        [Display(Name = "Mecánico")]
        public string NombreMecanico { get; set; }
    }

    public class CitaDetalleViewModel
    {
        public int Id { get; set; }

        public string NombreCliente { get; set; }
        public string TelefonoCliente { get; set; }

        public string Vehiculo { get; set; }
        public string Placa { get; set; }

        public DateTime FechaCita { get; set; }
        public string Estado { get; set; }

        public string NombreMecanico { get; set; }
        public string NombreUsuario { get; set; }

        public string Motivo { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
        public bool TieneFotoAntes { get; set; }
        public bool TieneFotoDespues { get; set; }
    }


    public class CitaFinalizarViewModel
    {
        public int CitaId { get; set; }

        [Display(Name = "Foto antes")]
        public HttpPostedFileBase FotoAntes { get; set; }

        [Display(Name = "Foto después")]
        public HttpPostedFileBase FotoDespues { get; set; }

        [Display(Name = "Observaciones / Motivo")]
        public string Motivo { get; set; }
    }

    public class CitaEditarMecanicoViewModel
    {
        public int CitaId { get; set; }

        [Display(Name = "Observaciones / Detalle del trabajo realizado")]
        public string Motivo { get; set; }

        public bool TieneFotoAntes { get; set; }
        public bool TieneFotoDespues { get; set; }

        [Display(Name = "Reemplazar foto antes")]
        public HttpPostedFileBase FotoAntes { get; set; }

        [Display(Name = "Reemplazar foto después")]
        public HttpPostedFileBase FotoDespues { get; set; }
    }

}