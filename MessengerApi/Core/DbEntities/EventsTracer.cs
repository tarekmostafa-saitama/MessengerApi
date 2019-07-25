using System;
using MessengerApi.Core.Enums;

namespace MessengerApi.Core.DbEntities
{
    public class EventsTracer
    {
        public int Id { get; set; }

        public EventType EventCode { get; set; }

        public DateTime Date { get; set; }

    }
}