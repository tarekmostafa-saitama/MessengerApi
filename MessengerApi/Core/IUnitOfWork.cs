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
        IImageRepository ImageRepository { get; }
        IAnonymousHubDataRepository AnonymousHubDataRepository { get; }
        IMemberHubDataRepository MemberHubDataRepository { get; }
        IVideoHubDataRepository VideoHubDataRepository { get; }
        void Complete();
    }
}