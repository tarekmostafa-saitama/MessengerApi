using System;

namespace MessengerApi.Core.Repositories
{
    public interface IMemberHubDataRepository
    {
        void AddToOnlineMembers(string connectionId,string userName);
        bool CheckKeyExistOnlineMembers(string key);
        bool CheckValueExistOnlineMembers(string value);
        bool RemoveFromOnlineMembers(string key);
        void AddToOnlineFriends(string connectionId, Guid relationId);
        Guid GetValueFromOnlineFriends(string key);
        bool CheckKeyExistOnlineFriends(string key);
        bool CheckValueExistOnlineFriends(Guid value);
        bool RemoveFromOnlineFriends(string key);
    }
}