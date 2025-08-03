using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace FastParts.Models
{
    public class EncuestaViewModel
    {
        public int IdPedido { get; set; }  // opcional, para asociar al servicio recibido

        [Required]
        [Range(1, 5)]
        public int CalificacionServicio { get; set; }

        [Required]
        [Range(1, 5)]
        public int CalificacionTiempo { get; set; }

        [Required]
        [Range(1, 5)]
        public int CalificacionTrato { get; set; }

        public string ComentariosAdicionales { get; set; }
    }
}
