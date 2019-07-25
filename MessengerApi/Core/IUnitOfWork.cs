using MessengerApi.Core.Repositories;

namespace MessengerApi.Core
{
    public interface IUnitOfWork
    {
        IMembersRepository MembersRepository { get; }
        IMessagesRepository MessagesRepository { get; }
        IRelationsRepository RelationsRepository { get; }
        IEventTracerRepository EventTracerRepository { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        void Complete();
    }
}