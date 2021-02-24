using ESAWebApplication.Utils;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ESAWebApplication.Startup))]

namespace ESAWebApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {           
        }
    }
}
