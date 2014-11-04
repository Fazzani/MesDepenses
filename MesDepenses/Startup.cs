using Microsoft.Owin;
using Owin;
namespace MesDepenses
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}