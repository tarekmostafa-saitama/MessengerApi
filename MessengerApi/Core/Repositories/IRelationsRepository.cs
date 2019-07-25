using System;
using System.Collections.Generic;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.Repositories
{
    public interface IRelationsRepository
    {
        Relation GetRelation(Guid id);
        void RemoveRelation(Relation relation);
        IEnumerable<Relation> GetRelations(string userId);
        Relation GetRelationByKey(string userId, string Key);
        void AddRelation(Relation relation);
    }
}