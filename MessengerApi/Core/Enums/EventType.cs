using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerApi.Core.Enums
{
    public enum EventType
    {

        UserRegistered = 1,
        MemberMessage = 2,
        StrangerMessage = 3,
        ErrorHappen =4
    }
}