using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BlogNoticias.Startup))]

namespace BlogNoticias
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
