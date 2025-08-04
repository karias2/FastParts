using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FastParts.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public List<string> Roles { get; set; } = new List<string>();
    }
}