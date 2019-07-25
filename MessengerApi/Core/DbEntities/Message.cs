using System;
using MessengerApi.Core.Enums;

namespace MessengerApi.Core.DbEntities
{
    public class Message
    {
        public int Id { get; set; }
        public SenderType Sender { get; set; }
        public MessageType Type { get; set; }
        public string MessageData { get; set; }
        public DateTime Date { get; set; }
        public bool IsReaded { get; set; }
        public Guid relation_id { get; set; }
        public Relation Relation { get; set; }
    }
}