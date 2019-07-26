using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MessengerApi.Core;
using MessengerApi.Core.Enums;
using Microsoft.AspNet.SignalR;

namespace MessengerApi.Hubs
{
    public class AnonymousHub : Hub
    {


        // DI ???
        private readonly IUnitOfWork _unitOfWork ;
        public AnonymousHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Routing(string msg)
        {
            msg = HttpUtility.HtmlEncode(msg);
            foreach (KeyValuePair<string, string> i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    if (i.Key != Context.ConnectionId)
                    {
                        Clients.Client(i.Key).strangerMessage(new { Type = "T", Message = msg, Date = DateTime.UtcNow, Sender = "Stranger" });
                    }
                    else
                    {
                        Clients.Client(i.Value).strangerMessage(new { Type = "T", Message = msg, Date = DateTime.UtcNow, Sender = "Stranger" });
                    }
                }
            }
            _unitOfWork.EventTracerRepository.AddEvent(EventType.StrangerMessage);
            _unitOfWork.Complete();
        }
        public void RoutingImages(string msg)
        {

            foreach (KeyValuePair<string, string> i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    if (i.Key != Context.ConnectionId)
                    {
                        Clients.Client(i.Key).strangerMessage(new { Type = "I", Message = msg, Date = DateTime.UtcNow, Sender = "Stranger" });
                    }
                    else
                    {
                        Clients.Client(i.Value).strangerMessage(new { Type = "I", Message = msg, Date = DateTime.UtcNow, Sender = "Stranger" });
                    }
                }
            }
            _unitOfWork.EventTracerRepository.AddEvent(EventType.StrangerMessage);
            _unitOfWork.Complete();
    
        }
        public void RegisterAndConnect()
        {
            bool status = ConnectToStranger();
            if (!status)
            {
                RegisterToWaitingList();
                Clients.Caller.serverMessage(new { Message = "waitingForStranger", Date = DateTime.UtcNow, Sender = "Server" });
            }
        }
       
        public override Task OnConnected()
        {
            //if (!TokenResolver.Resolve(Context.QueryString["token"]).Authorized)
            //{
            //    // Logout User
            //    return null;
            //}

      

            RegisterAndConnect();
            return base.OnConnected();
        }
        public bool ConnectToStranger()
        {
            var callerId = Context.ConnectionId;
            var connectionToConnect = _unitOfWork.AnonymousHubDataRepository.GetFirstWaitingList();
            if (connectionToConnect != null && connectionToConnect != Context.ConnectionId)
            {
                _unitOfWork.AnonymousHubDataRepository.RemoveFirstWaitingList();
                _unitOfWork.AnonymousHubDataRepository.AddToPairsData(connectionToConnect,callerId);
                var temp = new List<string>
                {
                    connectionToConnect,
                    Context.ConnectionId
                };
                Clients.Clients(temp).serverMessage(new { Message = "connectedtoStranger", Date = DateTime.UtcNow, Sender = "Server" }); ;
                return true;
            }

            return false;
        }
        public bool RegisterToWaitingList()
        {
            var callerId = Context.ConnectionId;
            _unitOfWork.AnonymousHubDataRepository.AddToWaitingList(callerId);
            return true;
        }
        public override Task OnReconnected()
        {
            foreach (KeyValuePair<string, string> i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    if (i.Key != Context.ConnectionId)
                    {
                        Clients.Client(i.Key).serverMessage(new { Message = "reconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ;
                    }
                    else
                    {
                        Clients.Client(i.Value).serverMessage(new { Message = "reconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ;
                    }

                }
            }
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Leave();
            return base.OnDisconnected(stopCalled);
        }
        public void Leave()
        {
            bool status = LeaveFromPairs();

            if (!status)
            {
                LeaveFromWaitingList();
            }
        }
        public bool LeaveFromPairs()
        {
            foreach (var i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    var temp = new List<string> {i.Value, i.Key};
                    Clients.Clients(temp).serverMessage(new { Message = "disconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ; ;
                    Clients.Clients(temp).changeTyping(false);

                    _unitOfWork.AnonymousHubDataRepository.RemoveFromPairsData(i.Key);
                    return true;
                }
            }
            return false;
        }
        public bool LeaveFromWaitingList()
        {

            if (_unitOfWork.AnonymousHubDataRepository.CheckExistingWaitingList(Context.ConnectionId))
            {
                _unitOfWork.AnonymousHubDataRepository.RemoveFromWaitingList(Context.ConnectionId);
                return true;
            }
            return false;
        }
        public void Typing(bool state)
        {
            foreach (KeyValuePair<string, string> i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    if (i.Key != Context.ConnectionId)
                    {

                        Clients.Client(i.Key).changeTyping(state);

                    }
                    else
                    {

                        Clients.Client(i.Value).changeTyping(state);


                    }
                }
            }
        }
    }
    }