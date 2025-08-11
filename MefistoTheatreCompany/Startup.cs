using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MefistoTheatreCompany.Startup))]
namespace MefistoTheatreCompany
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
