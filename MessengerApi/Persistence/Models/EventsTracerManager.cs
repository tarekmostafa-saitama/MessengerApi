using System;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Enums;
using MessengerApi.Persistence.Identity;

namespace MessengerApi.Persistence.Models
{
    public class EventsTracerManager
    {
        public static void AddEvent(EventType eventType)
        {
            var db = new ApplicationDbContext();
            db.EventsTracer.Add(new EventsTracer { EventCode = eventType, Date = DateTime.UtcNow });
            db.SaveChanges();
        }
    }
}