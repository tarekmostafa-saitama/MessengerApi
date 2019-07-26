using MessangerApi.Core.Repositories;
using MessengerApi.Core.Repositories;
using MessengerApi.Persistence.Repositories;

namespace MessengerApi.Core
{
    public interface IUnitOfWork
    {
        IMembersRepository MembersRepository { get; }
        IMessagesRepository MessagesRepository { get; }
        IRelationsRepository RelationsRepository { get; }
        IEventTracerRepository EventTracerRepository { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        IImageRepository ImageRepository { get; }
        IAnonymousHubDataRepository AnonymousHubDataRepository { get; }
        void Complete();
    }
}