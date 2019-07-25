using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MessengerAPI.Hubs
{
    public class MainHub : Hub
    {


        #region Helpers
        private object GetAuthInfo()
        {
            var user = Context.User;
            return new
            {
                IsAuthenticated = user.Identity.IsAuthenticated,
                IsMember = user.IsInRole("Member"),
                UserName = user.Identity.Name
            };
        }
        #endregion
    }
}