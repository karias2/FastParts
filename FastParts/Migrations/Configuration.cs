namespace FastParts.Migrations
{
    using FastParts.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<FastParts.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FastParts.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Mecanico"))
            {
                var SeedUsuarios = new SeedUsuarios();
                SeedUsuarios.CrearRolesYUsuarios(context);
            }

            if (!context.Encuestas.Any())
            {
                var SeedEncuenta = new SeedEncuestas();
                SeedEncuenta.Satisfaccion(context);
            }

            var seedRepuestos = new SeedRepuestos();
            seedRepuestos.addRepuestos(context);

            var seedServicios = new SeedServicios();
            seedServicios.AddServicios(context);
        }
    }
}
