using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class CitaModel
    {
        public int Id { get; set; } 
        public string UsuarioId { get; set; } 
        public string NombreCliente { get; set; } 
        public string TelefonoCliente { get; set; } 

        public string Vehiculo { get; set; } 
        public string Placa { get; set; } 

        public DateTime FechaCita { get; set; } 
        public string Motivo { get; set; } 

        public string MecanicoId { get; set; } 
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
    
        public byte[] FotoAntes { get; set; }
        public byte[] FotoDespues { get; set; }

        public string FotoAntesContentType { get; set; }
        public string FotoDespuesContentType { get; set; }


        //Estado de cita (Ingresada, Asignada, En Proceso, Terminada)
        public string Estado { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser Usuario { get; set; }

        [ForeignKey("MecanicoId")]
        public virtual ApplicationUser Mecanico { get; set; }

        // Servicios y Repuestos asociados a la cita
        public virtual ICollection<CitaServicioModel> Servicios { get; set; }
        public virtual ICollection<CitaRepuestoModel> Repuestos { get; set; }
    }
}