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

            //
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // RoleManager por solicitud
            app.CreatePerOwinContext<RoleManager<IdentityRole>>((options, context) =>
                new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>())));
            //

        }
    }
}
