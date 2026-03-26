using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastParts.Models
{
    public class EncuestaServicioModel
    {
        public int Id { get; set; }

        [Required]
        public int CitaId { get; set; }

        [Required]
        public string ClienteId { get; set; }

        public string MecanicoId { get; set; }

        [Range(1, 5)]
        [Display(Name = "Calificación general del servicio")]
        public int? CalificacionGeneral { get; set; }

        [Range(1, 5)]
        [Display(Name = "Calificación de la atención del mecánico")]
        public int? CalificacionMecanico { get; set; }

        [Display(Name = "¿Recomendaría el taller?")]
        public bool? RecomendariaTaller { get; set; }

        [Display(Name = "Comentario adicional")]
        [StringLength(500)]
        public string Comentario { get; set; }

        public DateTime? FechaRespuesta { get; set; }
        public bool Respondida { get; set; }

        [ForeignKey("CitaId")]
        public virtual CitaModel Cita { get; set; }

        [ForeignKey("ClienteId")]
        public virtual ApplicationUser Cliente { get; set; }

        [ForeignKey("MecanicoId")]
        public virtual ApplicationUser Mecanico { get; set; }
    }

    public class EncuestaServicioListadoViewModel
    {
        public int Id { get; set; }
        public int CitaId { get; set; }
        public DateTime FechaCita { get; set; }
        public string Vehiculo { get; set; }
        public string Placa { get; set; }
        public string NombreMecanico { get; set; }
        public bool Respondida { get; set; }
        public DateTime? FechaRespuesta { get; set; }
    }

    public class ResponderEncuestaViewModel
    {
        public int EncuestaId { get; set; }
        public int CitaId { get; set; }

        public DateTime FechaCita { get; set; }
        public string Vehiculo { get; set; }
        public string Placa { get; set; }
        public string NombreMecanico { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "Calificación general del servicio")]
        public int CalificacionGeneral { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "Calificación de la atención del mecánico")]
        public int CalificacionMecanico { get; set; }

        [Required]
        [Display(Name = "¿Recomendaría el taller?")]
        public bool RecomendariaTaller { get; set; }

        [Display(Name = "Comentario adicional")]
        [StringLength(500)]
        public string Comentario { get; set; }
    }

    public class ResultadoEncuestaAdminViewModel
    {
        public int Id { get; set; }
        public int CitaId { get; set; }
        public string Cliente { get; set; }
        public string Mecanico { get; set; }
        public string Vehiculo { get; set; }
        public DateTime FechaCita { get; set; }
        public int? CalificacionGeneral { get; set; }
        public int? CalificacionMecanico { get; set; }
        public bool? RecomendariaTaller { get; set; }
        public string Comentario { get; set; }
        public DateTime? FechaRespuesta { get; set; }
    }
}