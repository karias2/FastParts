using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FastParts.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Nombre Completo")]
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public bool Estado { get; set; }
        //public List<string> Roles { get; set; } = new List<string>();
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        // Modelos a registrar en la base de datos
        public DbSet<EncuestaModel> Encuestas { get; set; }
        public DbSet<PreguntaModel> Preguntas { get; set; }
        public DbSet<RepuestoModel> Repuestos { get; set; }
        public DbSet<ServicioRepuestoModel> ServicioRepuestos { get; set; }
        public DbSet<MovimientoInventarioModel> Movimientos { get; set; }
        public DbSet<AlertaInventarioModel> Alertas { get; set; }



        public System.Data.Entity.DbSet<FastParts.Models.ServicioModel> ServicioModels { get; set; }

        //public System.Data.Entity.DbSet<FastParts.Models.Servicio> Servicios { get; set; }
    }
}

