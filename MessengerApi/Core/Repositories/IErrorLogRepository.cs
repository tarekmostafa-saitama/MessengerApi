using System.Collections.Generic;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.Repositories
{
    public interface IErrorLogRepository
    {
        void SubmitError(ErrorsLog error);
        IEnumerable<ErrorsLog> GetError();
    }
}