using System;

namespace MessengerApi.Core.DataTransferObjects
{
    public class FriendRelationDTO
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastSeen { get; set; }
        // public string UserId { get; set; }
        public bool State { get; set; }
    }
}