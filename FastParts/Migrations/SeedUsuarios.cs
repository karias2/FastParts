using FastParts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastParts.Migrations
{
    public class SeedUsuarios
    {
        public void CrearRolesYUsuarios(FastParts.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));
            if (!roleManager.RoleExists("Mecanico"))
                roleManager.Create(new IdentityRole("Mecanico"));
            if (!roleManager.RoleExists("Cliente"))
                roleManager.Create(new IdentityRole("Cliente"));

            if (userManager.FindByName("admin@fastparts.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@fastparts.com",
                    Email = "admin@fastparts.com",
                    NombreCompleto = "Usuario Admin Por Defecto",
                    Estado = true
                };
                var result = userManager.Create(user, "Admin123");
                if (result.Succeeded) userManager.AddToRole(user.Id, "Admin");
            }

            if (userManager.FindByName("mecanico@fastparts.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "mecanico@fastparts.com",
                    Email = "mecanico@fastparts.com",
                    NombreCompleto = "Usuario Mecanico Por Defecto",
                    Estado = true
                };
                var result = userManager.Create(user, "Mecanico123");
                if (result.Succeeded) userManager.AddToRole(user.Id, "Mecanico");
            }

            if (userManager.FindByName("cliente@fastparts.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "cliente@fastparts.com",
                    Email = "cliente@fastparts.com",
                    NombreCompleto = "Usuario Cliente Por Defecto",
                    Estado = true
                };
                var result = userManager.Create(user, "Cliente123");
                if (result.Succeeded) userManager.AddToRole(user.Id, "Cliente");
            }
        }
    }
}