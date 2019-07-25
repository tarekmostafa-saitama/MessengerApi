using System;

namespace MessengerApi.Core.DataTransferObjects
{
    public class MessageDTO
    {
        public string MessageData { get; set; }
        public string Sender { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
    }
}