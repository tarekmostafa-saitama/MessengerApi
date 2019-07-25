using System.Collections.Generic;
using System.Linq;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Repositories;
using MessengerApi.Persistence.Identity;

namespace MessengerApi.Persistence.Repositories
{
    public class MembersRepository : IMembersRepository
    {
        private readonly ApplicationDbContext _context;

        public MembersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetUser(string name)
        {
            return _context.Users.First(x => x.UserName == name);
        }
        public bool UserExist(string name)
        {
            return _context.Users.Count(x => x.UserName == name) == 1;
        }

        public IEnumerable<ApplicationUser> SearchMembers(string name)
        {
            return _context.Users.Where(x => x.FullName.Contains(name) && x.AppearInSearch == true).ToList();
        }
    }
}