using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebFeedReader.Startup))]
namespace WebFeedReader
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
