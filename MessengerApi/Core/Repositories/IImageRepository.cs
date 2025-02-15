﻿using System.Web;

namespace MessengerApi.Core.Repositories
{
    public interface IImageRepository
    {
        string SaveMemberImageMessage(HttpPostedFile file,string id);
        string SaveMemberProfileImage();
        string SaveStrangerImageMessage(HttpPostedFile file);
        string DeleteMemberImageMessage();
        string DeleteMemberProfileImage();
        string EraseStrangerImageFolder();
    }
}