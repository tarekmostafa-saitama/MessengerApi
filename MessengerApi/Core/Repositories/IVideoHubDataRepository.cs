using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerApi.Core.Repositories
{
    public interface IVideoHubDataRepository
    {
        void AddToWaitingList(string signalrId,string webrtcId);
        void RemoveFromWaitingList(string signalrId);
        bool CheckExistingWaitingList(string signalrId);
        string GetFirstWaitingListKey();
        string GetFirstWaitingListValue();
        void RemoveFirstWaitingList(string signalrId);
        Dictionary<string, string> GetPairsData();
        void RemoveFromPairsData(string key);
        void AddToPairsData(string key, string value);
    }
}