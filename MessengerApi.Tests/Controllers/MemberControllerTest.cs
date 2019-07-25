using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessengerApi.Tests.Controllers
{
    [TestClass]
    public class MemberControllerTest
    {
        public MemberControllerTest()
        {
            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "TestUsername"));
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "1"));

            var principal = new GenericPrincipal(identity,null);

        }

    }
}
