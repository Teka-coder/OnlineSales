using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HagbesOnlineSales.Startup))]
namespace HagbesOnlineSales
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
