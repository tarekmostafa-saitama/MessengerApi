
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MessengerApi.Core;
using MessengerApi.Persistence.Models;
using MessengerApi.Persistence.Models.IdentitySecurityHelpers;
using Microsoft.AspNet.SignalR;

namespace MessengerAPI.Hubs
{
    public class HandlingPipelineModule :HubPipelineModule
    {

        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            string token = invokerContext.Hub.Context.QueryString["token"];
            ErrorLogger.Log(exceptionContext.Error,TokenResolver.Resolve(token).UserName);
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}