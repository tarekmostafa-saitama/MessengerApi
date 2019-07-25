using System.Collections.Generic;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.Repositories
{
    public interface IMembersRepository
    {
        ApplicationUser GetUser(string name);
        bool UserExist(string name);
        IEnumerable<ApplicationUser> SearchMembers(string name);
    }
}