using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MessengerApi.Core.Repositories;

namespace MessengerApi.Persistence.Repositories
{
    public class VideoHubDataRepository:IVideoHubDataRepository
    {
        private static Dictionary<string, string> _waitingList = new Dictionary<string, string>();
        private static Dictionary<string, string> _pairs = new Dictionary<string, string>();
        public VideoHubDataRepository()
        {

        }

        public void AddToWaitingList(string signalrId, string webrtcId)
        {
            _waitingList.Add(signalrId, webrtcId);
        }
        public void RemoveFromWaitingList(string signalrId)
        {
            _waitingList.Remove(signalrId);
        }
        public bool CheckExistingWaitingList(string signalrId)
        {
            return _waitingList.ContainsKey(signalrId);
        }
        public string GetFirstWaitingListKey()
        {
            return _waitingList.FirstOrDefault().Key;
        }
        public string GetFirstWaitingListValue()
        {
            return _waitingList.FirstOrDefault().Value;
        }
        public void RemoveFirstWaitingList(string signalrId)
        {
            _waitingList.Remove(signalrId);
        }

        public Dictionary<string, string> GetPairsData()
        {
            return _pairs;
        }
        public void RemoveFromPairsData(string key)
        {
            _pairs.Remove(key);
        }
        public void AddToPairsData(string key, string value)
        {
            _pairs.Add(key, value);
        }

    }
}