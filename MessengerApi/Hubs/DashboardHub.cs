using System.Threading.Tasks;
using MessengerApi.Persistence.Models.IdentitySecurityHelpers;
using Microsoft.AspNet.SignalR;

namespace MessengerApi.Hubs
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