using MessangerApi.Core.Repositories;
using MessengerApi.Core;
using MessengerApi.Core.Repositories;
using MessengerApi.Persistence.Identity;
using MessengerApi.Persistence.Repositories;

namespace MessengerApi.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IMembersRepository MembersRepository { get; private set; }
        public IMessagesRepository MessagesRepository { get; private set; }
        public IRelationsRepository RelationsRepository { get; private set; }
        public IEventTracerRepository EventTracerRepository { get; private set; }
        public IErrorLogRepository ErrorLogRepository { get; private set; }
        public IImageRepository ImageRepository { get; private set; }
        public IAnonymousHubDataRepository AnonymousHubDataRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext context,IFileHandlerRepository fileHandlerRepository)
        {
            _context = context;
            MembersRepository = new MembersRepository(context);
            MessagesRepository = new MessagesRepository(context);
            RelationsRepository = new RelationsRepository(context);
            EventTracerRepository = new EventTracerRepository(context);
            ErrorLogRepository = new ErrorLogRepository(context);
            ImageRepository = new ImageRepository(fileHandlerRepository);
            AnonymousHubDataRepository = new AnonymousHubDataRepository();
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}