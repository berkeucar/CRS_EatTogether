using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EatTogether.Startup))]
namespace EatTogether
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
