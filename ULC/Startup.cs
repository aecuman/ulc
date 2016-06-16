using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ULC.Startup))]
namespace ULC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
