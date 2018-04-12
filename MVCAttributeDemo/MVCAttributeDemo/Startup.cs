using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCAttributeDemo.Startup))]
namespace MVCAttributeDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
