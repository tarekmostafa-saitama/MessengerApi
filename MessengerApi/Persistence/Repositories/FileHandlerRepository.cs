using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerApi.Persistence.Repositories
{
    public class FileHandlerRepository
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