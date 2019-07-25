using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using MessengerAPI.Hubs;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(MessengerApi.Startup))]

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
