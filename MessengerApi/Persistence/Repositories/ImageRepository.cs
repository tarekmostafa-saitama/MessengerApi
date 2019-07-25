using System;
using System.IO;
using System.Web;
using MessangerApi.Core.Repositories;

namespace MessengerApi.Persistence.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IFileHandlerRepository _fileHandler;

        public ImageRepository(IFileHandlerRepository fileHandlerRepository)
        {
            _fileHandler = fileHandlerRepository;
        }

        public string SaveMemberImageMessage(HttpPostedFile file,string id)
        {
            var newName = id + "@" + Guid.NewGuid().ToString();
            var extenstion = Path.GetExtension(file.FileName);
            string  path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/MemberMessages/"), newName + extenstion);
            _fileHandler.SaveFile(file,path);
            path = "Content/MemberMessages/" + newName + Path.GetExtension(file.FileName);
            return path;
        }
        public string SaveMemberProfileImage()
        {
            return "";
        }
        public string SaveStrangerImageMessage(HttpPostedFile file)
        {
            var newName = Guid.NewGuid().ToString();
            var extenstion = Path.GetExtension(file.FileName);
            var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/StrangerMessages/"), newName + extenstion);
            _fileHandler.SaveFile(file, path);
            path = "Content/StrangerMessages/" + newName + Path.GetExtension(file.FileName);
            return path;
        }
        public string DeleteMemberImageMessage()
        {
            return "";
        }
        public string DeleteMemberProfileImage()
        {
            return "";
        }
        public string EraseStrangerImageFolder()
        {
            return "";
        }
    }
}