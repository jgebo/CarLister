using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarLister.Startup))]
namespace CarLister
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
