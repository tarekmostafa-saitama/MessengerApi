using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using MessengerApi.Core;
using MessengerApi.Core.Enums;
using MessengerApi.Persistence;
using MessengerApi.Persistence.Identity;
using MessengerApi.Persistence.Models;

namespace MessengerAPI.Hubs
{
    public class AnonymousHub : Hub
    {

        private static List<string> _waitingList = new List<string>();
        private static Dictionary<string, string> _pairs = new Dictionary<string, string>();
        // DI ???
        private readonly IUnitOfWork _unitOfWork ;
        public AnonymousHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Routing(string msg)
        {
            msg = HttpUtility.HtmlEncode(msg);
            foreach (KeyValuePair<string, string> i in _pairs)
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

            foreach (KeyValuePair<string, string> i in _pairs)
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

            string CallerId = Context.ConnectionId;
            string ConnectionToConnect = _waitingList.FirstOrDefault();
            if (ConnectionToConnect != null && ConnectionToConnect != Context.ConnectionId)
            {
                _waitingList.RemoveAt(0);
                _pairs.Add(ConnectionToConnect, CallerId);
                List<string> temp = new List<string>();
                temp.Add(ConnectionToConnect);
                temp.Add(Context.ConnectionId);
                Clients.Clients(temp).serverMessage(new { Message = "connectedtoStranger", Date = DateTime.UtcNow, Sender = "Server" }); ;
                return true;
            }

            return false;
        }
        public bool RegisterToWaitingList()
        {
            string CallerId = Context.ConnectionId;
            _waitingList.Add(CallerId);
            return true;
        }
        public override Task OnReconnected()
        {
            foreach (KeyValuePair<string, string> i in _pairs)
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
            foreach (KeyValuePair<string, string> i in _pairs)
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    List<string> temp = new List<string>();
                    temp.Add(i.Value);
                    temp.Add(i.Key);
                    Clients.Clients(temp).serverMessage(new { Message = "disconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ; ;
                    Clients.Clients(temp).changeTyping(false);

                    _pairs.Remove(i.Key);
                    return true;
                }
            }
            return false;
        }
        public bool LeaveFromWaitingList()
        {

            if (_waitingList.Contains(Context.ConnectionId))
            {
                _waitingList.Remove(Context.ConnectionId);
                return true;
            }
            return false;
        }
        public void Typing(bool state)
        {
            foreach (KeyValuePair<string, string> i in _pairs)
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