using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(spLettings.Startup))]
namespace spLettings
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
