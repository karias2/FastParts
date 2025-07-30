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
        }
    }
}
