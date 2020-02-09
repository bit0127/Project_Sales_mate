using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalesERP.Startup))]
namespace SalesERP
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
