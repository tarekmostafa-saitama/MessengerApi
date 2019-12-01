using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Castle.Core.Internal;
using MessengerApi.Core;
using MessengerApi.Core.Enums;
using Microsoft.AspNet.SignalR;

namespace MessengerApi.Hubs
{
    public class VideoHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        public VideoHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Setup(string webrtcId)
        {
            return RegisterAndConnect(webrtcId);
        }
        public string RegisterAndConnect(string webrtcId)
        {
            string status = ConnectToStranger();
            if (status.IsNullOrEmpty())
            {
                RegisterToWaitingList(webrtcId);
            }

            return status;
        }

        public override Task OnConnected()
        {
            

            return base.OnConnected();
        }
        public string ConnectToStranger()
        {
            var callerId = Context.ConnectionId;
            var connectionToConnect = _unitOfWork.VideoHubDataRepository.GetFirstWaitingListKey();
            if (connectionToConnect != null && connectionToConnect != Context.ConnectionId)
            {
                var webrtcId = _unitOfWork.VideoHubDataRepository.GetFirstWaitingListValue();
                _unitOfWork.VideoHubDataRepository.RemoveFirstWaitingList(connectionToConnect);
              return webrtcId;
            }

            return null;
        }
        public bool RegisterToWaitingList(string webrtcId)
        {
            var callerId = Context.ConnectionId;
            _unitOfWork.VideoHubDataRepository.AddToWaitingList(callerId, webrtcId);
            return true;
        }
        public override Task OnReconnected()
        {
            //foreach (KeyValuePair<string, string> i in _unitOfWork.VideoHubDataRepository.GetPairsData())
            //{
            //    if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
            //    {
            //        if (i.Key != Context.ConnectionId)
            //        {
            //            Clients.Client(i.Key).serverMessage(new { Message = "reconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ;
            //        }
            //        else
            //        {
            //            Clients.Client(i.Value).serverMessage(new { Message = "reconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ;
            //        }

            //    }
            //}
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Leave();
            return base.OnDisconnected(stopCalled);
        }
        public void Leave()
        {
           // bool status = LeaveFromPairs();

            if (!false)
            {
                LeaveFromWaitingList();
            }
        }
        //public bool LeaveFromPairs()
        //{
        //    foreach (var i in _unitOfWork.VideoHubDataRepository.GetPairsData())
        //    {
        //        if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
        //        {
        //            var temp = new List<string> { i.Value, i.Key };
        //            Clients.Clients(temp).serverMessage(new { Message = "disconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ; ;
        //            Clients.Clients(temp).changeTyping(false);

        //            _unitOfWork.VideoHubDataRepository.RemoveFromPairsData(i.Key);
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public bool LeaveFromWaitingList()
        {

            if (_unitOfWork.VideoHubDataRepository.CheckExistingWaitingList(Context.ConnectionId))
            {
                _unitOfWork.VideoHubDataRepository.RemoveFromWaitingList(Context.ConnectionId);
                return true;
            }
            return false;
        }
    
    }
}