using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoPanic.Backend.Startup))]
namespace DemoPanic.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
