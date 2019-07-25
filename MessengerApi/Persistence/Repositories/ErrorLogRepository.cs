using System.Collections.Generic;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Repositories;
using MessengerApi.Persistence.Identity;

namespace MessengerApi.Persistence.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly ApplicationDbContext _context;
        public ErrorLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SubmitError(ErrorsLog error)
        {
            _context.LogFile.Add(error);
        }
        public IEnumerable<ErrorsLog> GetError()
        {
           return _context.LogFile;
        }
    }
}