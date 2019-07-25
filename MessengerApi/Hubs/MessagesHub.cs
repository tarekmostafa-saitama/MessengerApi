using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using MessengerApi.Core;
using MessengerApi.Core.DataTransferObjects;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Enums;
using MessengerApi.Persistence.Models;
using MessengerApi.Persistence.Models.IdentitySecurityHelpers;
using Microsoft.AspNet.SignalR;

namespace MessengerApi.Hubs
{
    public class MessagesHub : Hub
    {
        private static Dictionary<string, string> _onlineMembers = new Dictionary<string, string>();
        private static Dictionary<string, Guid> _onlineFriends = new Dictionary<string, Guid>();

        private static IUnitOfWork _unitOfWork;
        public MessagesHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void RegisterMemberForNotification(Guid RelationId)
        {
            Groups.Add(Context.ConnectionId, RelationId + "Member");
            Groups.Add(Context.ConnectionId, RelationId + "FriendStatus");
        }
        public IEnumerable<FriendRelationDTO> SetupMemberenvironment()
        {

            // Validate User
            string username;
            if (TokenResolver.Resolve(Context.QueryString["token"]).Authorized)
                username = TokenResolver.Resolve(Context.QueryString["token"]).UserName;
            else
            {
                // Logout User
                return null;
            }



            var user = _unitOfWork.MembersRepository.GetUser(username);
            if(user == null)
            {
                // Logout User
                return null;
            }
            // Add to Member Online List
            MemberOnline(username);
            IEnumerable<Relation> relations = _unitOfWork.RelationsRepository.GetRelations(user.Id);
            Groups.Add(Context.ConnectionId, username + "Member");

           
            IEnumerable<FriendRelationDTO> FriendList = Mapper.Map<IEnumerable<Relation>, IEnumerable<FriendRelationDTO>>(relations).Select(c=> { c.State = _onlineFriends.ContainsKey(c.Id.ToString()); return c; });
            foreach (Relation i in relations)
            {
                RegisterMemberForNotification(i.Id);
            }
            
            return FriendList;

        }
        public IEnumerable<MessageDTO> GetMemberChat(Guid relation)
        {
            var msgs = _unitOfWork.MessagesRepository.GetMessages(relation);
            return Mapper.Map<IEnumerable<Message>, IEnumerable<MessageDTO>>(msgs);
        }




        // Friend Section
        public Guid SetupFriendEnvironment(string secretKey, string userName)
        {


            Groups.Add(Context.ConnectionId, userName + "MemberStatus");

            bool State = _onlineMembers.ContainsValue(userName);
            var user =_unitOfWork.MembersRepository.GetUser(userName);
            if(user == null)
            {
                // User Not Found ??//
                // Redirect to Not Found Page
            }
            var info =  new {user.Image, Name = user.FullName, State };
            Clients.Caller.displayDetails(info);
            var relation = _unitOfWork.RelationsRepository.GetRelationByKey(user.Id,secretKey);
            if (relation != null)
            {
                return OldRelation(secretKey, user,relation);
            }

            return NewRelation(secretKey, user);

        }
        public Guid OldRelation( string secretKey, ApplicationUser user,Relation relation)
        {
            
           
            relation.LastSeen = DateTime.UtcNow;
           _unitOfWork.Complete();

            FriendOnline(relation.Id);
            var msgssource = _unitOfWork.MessagesRepository.GetMessages(relation.Id);
            var msgs = Mapper.Map<IEnumerable<Message>, IEnumerable<MessageDTO>>(msgssource);
            Clients.Caller.placeChat(msgs);

            Groups.Add(Context.ConnectionId, relation.Id + "Friend");
            return relation.Id;
        }
        public Guid NewRelation(string secretKey, ApplicationUser user)
        {
            Relation Relation = new Relation { Id = Guid.NewGuid(), user_id = user.Id, SecretKey = secretKey, LastSeen = DateTime.UtcNow, NickName = "Stranger Friend", StartDate = DateTime.UtcNow };
           _unitOfWork.RelationsRepository.AddRelation(Relation);
           _unitOfWork.Complete();

            Guid relid = Relation.Id;
            FriendOnline(relid);
            Clients.Group(user.UserName + "Member").newFriendRelation(new FriendRelationDTO { Id = Relation.Id, LastSeen = Relation.LastSeen, NickName = Relation.NickName, StartDate = Relation.StartDate, State = true });
            Groups.Add(Context.ConnectionId, relid + "Friend");
            Clients.Caller.placeChat(new List<MessageDTO>());

            return relid;

        }


        public void MemberOnline(string username)
        {
            _onlineMembers.Add(Context.ConnectionId, username);
            Clients.Group(username + "MemberStatus").changeState(true);
        }
        public void MemberOffline()
        {

            string  username = TokenResolver.Resolve(Context.QueryString["token"]).UserName;
       

            if (_onlineMembers.ContainsKey(Context.ConnectionId))
            {
                _onlineMembers.Remove(Context.ConnectionId);
                if (!_onlineMembers.ContainsValue(username))
                {
                    Clients.Group(username + "MemberStatus").changeState(false);
                }
            }


        }

        public void FriendOnline(Guid relationId)
        {
            _onlineFriends.Add(Context.ConnectionId, relationId);
            Clients.Group(relationId + "FriendStatus").changeState(true, relationId);
        }

        public void FriendOffline()
        {
            if (_onlineFriends.ContainsKey(Context.ConnectionId))
            {
                var relationid = _onlineFriends.First(x => x.Key == Context.ConnectionId).Value;
                _onlineFriends.Remove(Context.ConnectionId);

                if (!_onlineFriends.ContainsValue(relationid))
                {
                    Clients.Group(relationid + "FriendStatus").changeState(false, relationid);
                }
            }


        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {

            if (Context.Request.QueryString["Destination"] == "MessagesPage")
            {

                MemberOffline();
            }
            if (Context.Request.QueryString["Destination"] == "FriendPage")
            {
                FriendOffline();
            }

            return base.OnDisconnected(stopCalled);
        }


        public void RoutingMessage(Guid relationId, string message, SenderType sender)
        {
    
            if (_unitOfWork.RelationsRepository.GetRelation(relationId) != null)
            {
                string temp = HttpUtility.HtmlEncode(message);
                if (sender == SenderType.Friend)
                {
                    Clients.OthersInGroup(relationId + "Member").recieveMessage(relationId, temp, MessageType.TextMessage);
                }
                else
                {
                    Clients.OthersInGroup(relationId + "Friend").recieveMessage(relationId, temp, MessageType.TextMessage);
                }
                _unitOfWork.MessagesRepository.AddMessage(new Message { relation_id = relationId, Type = MessageType.TextMessage, MessageData = temp, Date = DateTime.UtcNow, Sender = sender });
                _unitOfWork.Complete();
            }
            EventsTracerManager.AddEvent(EventType.MemberMessage);
        }

        // Neeed Optimization
        public void RoutingImageMessage(Guid relationId, string path, SenderType sender)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            if (_unitOfWork.RelationsRepository.GetRelation(relationId) != null) 
            {
                if (sender == SenderType.Friend)
                {
                    context.Clients.Group(relationId + "Member").recieveMessage(relationId, path, MessageType.ImageMessage);
                }
                else
                {
                    context.Clients.Group(relationId + "Friend").recieveMessage(relationId, path, MessageType.ImageMessage);
                }

            }
            EventsTracerManager.AddEvent(EventType.MemberMessage);
        }

        public void RoutingTyping(Guid relationId, bool Flag, SenderType sender)
        {

            if (sender == SenderType.Friend)
            {
                Clients.OthersInGroup(relationId + "Member").typingIndicator(new { FlagIndicator = Flag, RelationId = relationId });
            }
            else
            {
                Clients.OthersInGroup(relationId + "Friend").typingIndicator(new { FlagIndicator = Flag, RelationId = relationId });
            }

        }

    }
}