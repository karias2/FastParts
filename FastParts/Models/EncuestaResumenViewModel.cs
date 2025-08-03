using System;
using System.Data.Entity;
using System.Linq;

namespace FastParts.Models
{
    public class EncuestaResumenViewModel
    {
        public int Id { get; set; }
        public int CalificacionServicio { get; set; }
        public int CalificacionTiempo { get; set; }
        public int CalificacionTrato { get; set; }
        public string ComentariosAdicionales { get; set; }
        public DateTime Fecha { get; set; }
    }

}