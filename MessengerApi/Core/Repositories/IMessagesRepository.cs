using System;
using System.Collections.Generic;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.Repositories
{
    public interface IMessagesRepository
    {
        IEnumerable<Message> GetMessages(Guid id);
        void RemoveMessages(List<Message> messages);
        void AddMessage(Message message);
    }
}