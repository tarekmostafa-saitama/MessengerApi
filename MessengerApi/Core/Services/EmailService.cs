﻿using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MessengerApi.Core.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}