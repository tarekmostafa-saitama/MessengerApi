using System;
using System.Web;
using MessengerApi.Core.Enums;

namespace MessengerApi.Core.ViewModels
{
    public class MemberChatImage
    {
        public HttpPostedFile Image { get; set; }
        public Guid RelationId { get; set; }
        public SenderType Sender { get; set; }
    }
}