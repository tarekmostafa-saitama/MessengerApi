using System;
using System.Collections.Generic;

namespace MessengerApi.Core.DbEntities
{
    public class Relation
    {

        public Guid Id { get; set; }
        public string NickName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastSeen { get; set; }
        public string SecretKey { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Message> Messages { get; set; }
    }
}