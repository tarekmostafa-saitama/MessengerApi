using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MessengerApi.Persistence.Models.IdentitySecurityHelpers;
using Microsoft.AspNet.SignalR;

namespace MessengerAPI.Hubs
{
    public class DashboardHub : Hub
    {
        public void Increase(string Code)
        {
            Clients.All.increaseCounts(Code);
        }

        public override Task OnConnected()
        {
            if (!TokenResolver.Resolve(Context.QueryString["token"]).Authorized || 
                TokenResolver.Resolve(Context.QueryString["token"]).Role != "Admin")
            {
                // Logout User
                return null;
            }
            return base.OnConnected();
        }
    }
}