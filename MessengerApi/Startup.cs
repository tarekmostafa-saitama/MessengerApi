using MessengerApi;
using MessengerApi.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace MessengerApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);


            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            // hubConfiguration.EnableJavaScriptProxies = false;

            GlobalHost.HubPipeline.AddModule(new HandlingPipelineModule());
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(hubConfiguration);
            });

        }
    }
}
