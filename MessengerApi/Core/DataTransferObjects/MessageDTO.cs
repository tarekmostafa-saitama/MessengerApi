using System;
using MessengerApi.Core.Enums;

namespace MessengerApi.Core.DataTransferObjects
{
    public class MessageDTO
    {
        public string MessageData { get; set; }
        public SenderType Sender { get; set; }
        public DateTime Date { get; set; }
        public MessageType Type { get; set; }
    }
}