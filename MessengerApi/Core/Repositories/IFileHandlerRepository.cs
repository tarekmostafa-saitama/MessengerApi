using System.Web;

namespace MessangerApi.Core.Repositories
{
    public interface IFileHandlerRepository
    {
        void SaveFile(HttpPostedFile file,string path);
        void DeleteFile();
        void EmptyFolder();
    }
}
