using System.Collections.Generic;

namespace MessangerApi.Core.Repositories
{
    public interface IAnonymousHubDataRepository
    {
        void AddToWaitingList(string connectionId);
        void RemoveFromWaitingList(string connectionId);
        bool CheckExistingWaitingList(string connectionId);
        string GetFirstWaitingList();
        void RemoveFirstWaitingList();
        Dictionary<string, string> GetPairsData();
        void RemoveFromPairsData(string key);
        void AddToPairsData(string key,string value);
    }
}