using System;
using System.Linq;
using System.Security.Claims;

namespace MessengerApi.Persistence.Models.IdentitySecurityHelpers
{   
    public static class TokenResolver
    {

        public static TokenTicket Resolve(string token)
        {
            var Ticket = new TokenTicket();

            var ticket = Startup.OAuthOptions.AccessTokenFormat.Unprotect(token);
            if (ticket == null || ticket.Properties.ExpiresUtc.HasValue && (ticket.Properties.ExpiresUtc.Value < DateTime.UtcNow))
            {
                Ticket.Authorized = false;
                Ticket.UserName = "Anonymous";
            }
            else
            {
                Ticket.Authorized = true;
                Ticket.UserName = ticket.Identity.Name;
                Ticket.Role = ticket.Identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            }


            //bool isAuth = ticket.Identity.IsAuthenticated;
          
            return Ticket;
        }
    }
}