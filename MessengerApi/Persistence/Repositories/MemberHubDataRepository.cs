﻿using System;
using System.Collections.Generic;
using System.Linq;
using MessengerApi.Core.Repositories;

namespace MessengerApi.Persistence.Repositories
{
    public class MemberHubDataRepository : IMemberHubDataRepository
    {
        private static Dictionary<string, string> _onlineMembers  = new Dictionary<string, string>();
        private static Dictionary<string, Guid> _onlineFriends = new Dictionary<string, Guid>() ;
        public MemberHubDataRepository()
        {
    
        }

        public void AddToOnlineMembers(string connectionId,string userName)
        {
            _onlineMembers.Add(connectionId,userName);
        }

        public bool CheckKeyExistOnlineMembers(string key)
        {
            return _onlineMembers.ContainsKey(key);
        }
        public bool CheckValueExistOnlineMembers(string value)
        {
            return _onlineMembers.ContainsValue(value);
        }
        public bool RemoveFromOnlineMembers(string key)
        {
            return _onlineMembers.Remove(key);
        }



        public void AddToOnlineFriends(string connectionId, Guid relationId)
        {
            _onlineFriends.Add(connectionId, relationId);
        }
        public Guid GetValueFromOnlineFriends(string key)
        {
            return _onlineFriends.First(x => x.Key == key).Value;
        }

        public bool CheckKeyExistOnlineFriends(string key)
        {
            return _onlineFriends.ContainsKey(key);
        }
        public bool CheckValueExistOnlineFriends(Guid value)
        {
            return _onlineFriends.ContainsValue(value);
        }
        public bool RemoveFromOnlineFriends(string key)
        {
            return _onlineFriends.Remove(key);
        }
    }
}