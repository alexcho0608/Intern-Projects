using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArticleSite.Startup))]
namespace ArticleSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
