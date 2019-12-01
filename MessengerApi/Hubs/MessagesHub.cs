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


        private static IUnitOfWork _unitOfWork;
        /// <summary>
        ///  Use Dependency to assign Unit of work Object
        /// </summary>
        /// <param name="unitOfWork">Contain All Repositories that you can work with.</param>
        public MessagesHub(IUnitOfWork unitOfWork) :base()
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        ///  Assign User(Member) To His Relation Notification Channel Groups To Receive Updates.
        /// </summary>
        /// <param name="relationId"></param>
        public void RegisterMemberForNotification(Guid relationId)
        {
            // Channel for chat Forward
            Groups.Add(Context.ConnectionId, relationId + "Member");
            // Channel for status Type (On/Off)
            Groups.Add(Context.ConnectionId, relationId + "FriendStatus");
        }
        /// <summary>
        /// 1- Check User Identity
        /// 2- Register for Notifications
        /// 3- Return Friend Relations List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FriendRelationDTO> SetupMemberEnvironment()
        {
            // Validate User
            string username;
            if (TokenResolver.Resolve(Context.QueryString["token"]).Authorized)
                username = TokenResolver.Resolve(Context.QueryString["token"]).UserName;
            else
            {
                throw new UnauthorizedAccessException("User is Unauthorized.");
            }
            var user = _unitOfWork.MembersRepository.GetUser(username);
            if(user == null)
            {
                throw new NullReferenceException("User is not existed.");
            }
            
            MemberOnline(username);

            IEnumerable<Relation> relations = _unitOfWork.RelationsRepository.GetRelations(user.Id);

            Groups.Add(Context.ConnectionId, username + "Member");

           
            IEnumerable<FriendRelationDTO> friendList = Mapper.Map<IEnumerable<Relation>, 
                IEnumerable<FriendRelationDTO>>(relations)
                .Select(c=> { c.State = _unitOfWork.MemberHubDataRepository.CheckValueExistOnlineFriends(c.Id); return c; });
            foreach (var i in relations)
            {
                RegisterMemberForNotification(i.Id);
            }
            
            return friendList;

        }
        /// <summary>
        ///  Fetch Messages of specific relationId.
        /// </summary>
        /// <param name="relationId">Relation Id to get messages</param>
        /// <returns></returns>
        public IEnumerable<MessageDTO> GetMemberChat(Guid relationId)
        {
            var messages = _unitOfWork.MessagesRepository.GetMessages(relationId);
            return Mapper.Map<IEnumerable<Message>, IEnumerable<MessageDTO>>(messages);
        }
        /// <summary>
        ///  1- Check user exist
        ///  2- Send user profile details
        ///  3- Check Relation Old/New 
        /// </summary>
        /// <param name="secretKey">Relation Secret Key</param>
        /// <param name="userName">User Name from route</param>
        /// <returns></returns>
        public Guid SetupFriendEnvironment(string secretKey, string userName)
        {
            Groups.Add(Context.ConnectionId, userName + "MemberStatus");

            bool state = _unitOfWork.MemberHubDataRepository.CheckValueExistOnlineMembers(userName);
            var user =_unitOfWork.MembersRepository.GetUser(userName);
            if(user == null)
            {
                throw new NullReferenceException("User not existed");
            }
            var info =  new {user.Image, Name = user.FullName, State = state };
            // Send profile Details
            Clients.Caller.displayDetails(info);
            var relation = _unitOfWork.RelationsRepository.GetRelationByKey(user.Id,secretKey);
            if (relation != null)
            {
                return OldRelation(relation);
            }

            return NewRelation(secretKey, user);

        }

        /// <summary>
        /// 1- adjust last seen date
        /// 2- make friend online state
        /// 3- get past messages
        /// 4- register friend for notification channel
        /// </summary>
        /// <param name="relation">exist relation object</param>
        /// <returns></returns>
        public Guid OldRelation(Relation relation)
        {
            relation.LastSeen = DateTime.UtcNow;
           _unitOfWork.Complete();

            FriendOnline(relation.Id);
            var messagesSource = _unitOfWork.MessagesRepository.GetMessages(relation.Id);
            var messages = Mapper.Map<IEnumerable<Message>, IEnumerable<MessageDTO>>(messagesSource);
            Clients.Caller.placeChat(messages);

            Groups.Add(Context.ConnectionId, relation.Id + "Friend");
            return relation.Id;
        }
        /// <summary>
        /// 1- create new relation
        /// 2- change friend online state
        /// 3- register friend for notification channel
        /// </summary>
        /// <param name="secretKey">friend secret key</param>
        /// <param name="user">the member himself you want to contact with</param>
        /// <returns></returns>
        public Guid NewRelation(string secretKey, ApplicationUser user)
        {
            Relation relation = new Relation { Id = Guid.NewGuid(), UserId= user.Id, SecretKey = secretKey, LastSeen = DateTime.UtcNow, NickName = "Stranger Friend", StartDate = DateTime.UtcNow };
           _unitOfWork.RelationsRepository.AddRelation(relation);
           _unitOfWork.Complete();

            Guid relationId = relation.Id;
            FriendOnline(relationId);
            Clients.Group(user.UserName + "Member").newFriendRelation(new FriendRelationDTO { Id = relation.Id, LastSeen = relation.LastSeen, NickName = relation.NickName, StartDate = relation.StartDate, State = true });
            Groups.Add(Context.ConnectionId, relationId + "Friend");
            Clients.Caller.placeChat(new List<MessageDTO>());

            return relationId;

        }

        /// <summary>
        ///  1- register member to online state
        ///  2- send notification that he is online
        /// </summary>
        /// <param name="userName">member user name</param>
        public void MemberOnline(string userName)
        {
            _unitOfWork.MemberHubDataRepository.AddToOnlineMembers(Context.ConnectionId,userName);
            Clients.Group(userName + "MemberStatus").changeState(true);
        }
        /// <summary>
        ///  1- register member to offline state
        ///  2- send notification that he is offline
        /// </summary>
        public void MemberOffline()
        {
            string  userName = TokenResolver.Resolve(Context.QueryString["token"]).UserName;
            if (_unitOfWork.MemberHubDataRepository.CheckKeyExistOnlineMembers(Context.ConnectionId))
            {
                _unitOfWork.MemberHubDataRepository.RemoveFromOnlineMembers(Context.ConnectionId);
                if (!_unitOfWork.MemberHubDataRepository.CheckValueExistOnlineMembers(userName))
                {
                    Clients.Group(userName + "MemberStatus").changeState(false);
                }
            }
        }
        /// <summary>
        ///  1- register friend to online state
        ///  2- send notification that he is online
        /// </summary>
        /// <param name="relationId">friend relation id</param>
        public void FriendOnline(Guid relationId)
        {
            _unitOfWork.MemberHubDataRepository.AddToOnlineFriends(Context.ConnectionId, relationId);
            Clients.Group(relationId + "FriendStatus").changeState(true, relationId);
        }
        /// <summary>
        ///  1- register friend to offline state
        ///  2- send notification that he is offline
        /// </summary>
        public void FriendOffline()
        {
            if (_unitOfWork.MemberHubDataRepository.CheckKeyExistOnlineFriends(Context.ConnectionId))
            {
                var relationId = _unitOfWork.MemberHubDataRepository.GetValueFromOnlineFriends(Context.ConnectionId);
                _unitOfWork.MemberHubDataRepository.RemoveFromOnlineFriends(Context.ConnectionId);
                if (!_unitOfWork.MemberHubDataRepository.CheckValueExistOnlineFriends(relationId))
                {
                    Clients.Group(relationId + "FriendStatus").changeState(false, relationId);
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