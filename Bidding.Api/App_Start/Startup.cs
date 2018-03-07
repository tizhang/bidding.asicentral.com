using Microsoft.Owin;
using Owin;

namespace Bidding.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}