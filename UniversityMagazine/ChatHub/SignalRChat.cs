using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(UniversityMagazine.ChatHub.SignalRChat))]

namespace UniversityMagazine.ChatHub
{
    public class SignalRChat
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }
    }
}
