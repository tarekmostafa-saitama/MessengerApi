using System.Web;
using MessangerApi.Core.Repositories;

namespace MessengerApi.Persistence.Repositories
{
    public class FileHandlerRepository :  IFileHandlerRepository
    {
        public FileHandlerRepository()
        {
            
        }

        public void SaveFile(HttpPostedFile file,string path)
        {
            file.SaveAs(path);
        }
        public void DeleteFile()
        {

        }
        public void EmptyFolder()
        {

        }
    }
}