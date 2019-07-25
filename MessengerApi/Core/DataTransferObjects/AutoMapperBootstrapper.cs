using AutoMapper;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.DataTransferObjects
{
    public  class AutoMapperBootstrapper : Profile
    {
            public AutoMapperBootstrapper ()
            {
                CreateMap<Relation, FriendRelationDTO>();
                CreateMap<Message, MessageDTO>();
                CreateMap<ApplicationUser, UserSearchDataDTO>();
                CreateMap<ApplicationUser, ProfileDetailsDTO>();
            }
   
    }
}