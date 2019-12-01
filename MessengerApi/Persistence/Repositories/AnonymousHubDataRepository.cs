using System.Collections.Generic;
using System.Linq;
using MessengerApi.Core.Repositories;

namespace MessengerApi.Persistence.Repositories
{
    public class AnonymousHubDataRepository :  IAnonymousHubDataRepository
    {
        private static List<string> _waitingList = new List<string>();
        private static Dictionary<string, string> _pairs = new Dictionary<string, string>() ;
        public AnonymousHubDataRepository()
        {
           
        }

        public void AddToWaitingList(string connectionId)
        {
            _waitingList.Add(connectionId);
        }
        public void RemoveFromWaitingList(string connectionId)
        {
            _waitingList.Remove(connectionId);
        }
        public bool CheckExistingWaitingList(string connectionId)
        {
            return _waitingList.Contains(connectionId);
        }
        public string GetFirstWaitingList()
        {
            return _waitingList.FirstOrDefault();
        }
        public void RemoveFirstWaitingList()
        {
             _waitingList.RemoveAt(0);
        }

        public Dictionary<string, string> GetPairsData()
        {
            return _pairs;
        }
        public void RemoveFromPairsData(string key)
        {
            _pairs.Remove(key);
        }
        public void AddToPairsData(string key,string value)
        {
            _pairs.Add(key,value);
        }

    }
}