using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Repositories;
using MessengerApi.Persistence.Identity;
using Ninject.Infrastructure.Language;

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