using System;
using System.Collections.Generic;
using System.Linq;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Repositories;
using MessengerApi.Persistence.Identity;

namespace MessengerApi.Persistence.Repositories
{
    public class RelationsRepository : IRelationsRepository
    {
        private readonly ApplicationDbContext _context;

        public RelationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Relation GetRelation(Guid id)
        {
            return _context.Relations.FirstOrDefault(x => x.Id == id);
        }
        public Relation GetRelationByKey(string userId,string key)
        {
            return _context.Relations.FirstOrDefault(x => x.user_id == userId && x.SecretKey == key);
        }
        public IEnumerable<Relation> GetRelations(string userId)
        {
            return _context.Relations.Where(x => x.user_id == userId);
        }
        public void RemoveRelation(Relation relation)
        {
            _context.Relations.Remove(relation);
        }
        public void AddRelation(Relation relation)
        {
            _context.Relations.Add(relation);
        }
    }
}