namespace FastParts.Migrations
{
    using FastParts.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FastParts.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FastParts.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            if (!context.Repuestos.Any())
            {
                context.Repuestos.Add(new RepuestoModel { Nombre = "Filtro de Aceite", Marca = "ACME", NumeroParte = "FA-100", Precio = 4500, Stock = 20, Proveedor = "Proveedor 1" });
                context.Repuestos.Add(new RepuestoModel { Nombre = "Bujía", Marca = "SparkX", NumeroParte = "BX-200", Precio = 2500, Stock = 100, Proveedor = "Proveedor 2" });
                context.SaveChanges();
            }

            if (!context.Encuestas.Any())
            {
                var SeedEncuenta = new SeedEncuentas();
                SeedEncuenta.Satisfaccion(context);
            }
        }
    }
}
