using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Enums;
using MessengerApi.Core.Repositories;
using MessengerApi.Persistence.Identity;

namespace MessengerApi.Persistence.Repositories
{
    public class EventTracerRepository : IEventTracerRepository
    {
        private readonly ApplicationDbContext _context;

        public EventTracerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GetEventCount(EventType eventType)
        {
            return _context.EventsTracer.Count(x => x.EventCode == eventType);
        }
        public void AddEvent(EventType eventType)
        {
            _context.EventsTracer.Add(new EventsTracer { EventCode = eventType, Date = DateTime.UtcNow });
        }
    }
}