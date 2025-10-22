using FastParts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FastParts.Startup))]
namespace FastParts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CrearRolesYUsuarios();

       
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.CreatePerOwinContext<RoleManager<IdentityRole>>((options, context) =>
                new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>())));
  

        }

      

        private void CrearRolesYUsuarios()
        {
            using (var context = ApplicationDbContext.Create())
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

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Mecanico");
        //        }
        //    }


        //    if (userManager.FindByName("cliente@fastparts.com") == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = "cliente@fastparts.com",
        //            Email = "cliente@fastparts.com",
        //            NombreCompleto = "Usuario Cliente Por Defecto",
        //        };

        //        var password = "Cliente123";
        //        var result = userManager.Create(user, password);

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Cliente");
        //        }
        //    }
        //}

        private void CrearRolesYUsuarios()
        {
            using (var context = ApplicationDbContext.Create())
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

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Mecanico");
        //        }
        //    }


        //    if (userManager.FindByName("cliente@fastparts.com") == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = "cliente@fastparts.com",
        //            Email = "cliente@fastparts.com",
        //            NombreCompleto = "Usuario Cliente Por Defecto",
        //        };

        //        var password = "Cliente123";
        //        var result = userManager.Create(user, password);

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Cliente");
        //        }
        //    }
        //}

        private void CrearRolesYUsuarios()
        {
            using (var context = ApplicationDbContext.Create())
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

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Mecanico");
        //        }
        //    }


        //    if (userManager.FindByName("cliente@fastparts.com") == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = "cliente@fastparts.com",
        //            Email = "cliente@fastparts.com",
        //            NombreCompleto = "Usuario Cliente Por Defecto",
        //        };

        //        var password = "Cliente123";
        //        var result = userManager.Create(user, password);

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Cliente");
        //        }
        //    }
        //}

        private void CrearRolesYUsuarios()
        {
            using (var context = ApplicationDbContext.Create())
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

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Mecanico");
        //        }
        //    }


        //    if (userManager.FindByName("cliente@fastparts.com") == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = "cliente@fastparts.com",
        //            Email = "cliente@fastparts.com",
        //            NombreCompleto = "Usuario Cliente Por Defecto",
        //        };

        //        var password = "Cliente123";
        //        var result = userManager.Create(user, password);

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Cliente");
        //        }
        //    }
        //}

        private void CrearRolesYUsuarios()
        {
            using (var context = ApplicationDbContext.Create())
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

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Mecanico");
        //        }
        //    }


        //    if (userManager.FindByName("cliente@fastparts.com") == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = "cliente@fastparts.com",
        //            Email = "cliente@fastparts.com",
        //            NombreCompleto = "Usuario Cliente Por Defecto",
        //        };

        //        var password = "Cliente123";
        //        var result = userManager.Create(user, password);

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Cliente");
        //        }
        //    }
        //}

        private void CrearRolesYUsuarios()
        {
            using (var context = ApplicationDbContext.Create())
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

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Mecanico");
        //        }
        //    }


        //    if (userManager.FindByName("cliente@fastparts.com") == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = "cliente@fastparts.com",
        //            Email = "cliente@fastparts.com",
        //            NombreCompleto = "Usuario Cliente Por Defecto",
        //        };

        //        var password = "Cliente123";
        //        var result = userManager.Create(user, password);

        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRole(user.Id, "Cliente");
        //        }
        //    }
        //}

        private void CrearRolesYUsuarios()
        {
            using (var context = ApplicationDbContext.Create())
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
}
