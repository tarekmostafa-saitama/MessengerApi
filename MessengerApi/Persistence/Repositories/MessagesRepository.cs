using System;
using System.Collections.Generic;
using System.Linq;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Repositories;
using MessengerApi.Persistence.Identity;

namespace MessengerApi.Persistence.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ApplicationDbContext _context;

        public MessagesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Message> GetMessages(Guid id)
        {
           return _context.Messages.Where(x => x.relation_id == id);
        }

        public void RemoveMessages(List<Message> messages)
        {
            _context.Messages.RemoveRange(messages);
        }
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }


    }
}