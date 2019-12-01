using System.Web;

namespace MessengerApi.Core.Repositories
{
    public interface IFileHandlerRepository
    {
        void SaveFile(HttpPostedFile file,string path);
        void DeleteFile();
        void EmptyFolder();
    }
}
