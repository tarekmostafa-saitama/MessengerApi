using System;
using System.Collections.Generic;
using MessengerApi.Core.DbEntities;
using MessengerApi.Persistence.Identity;

namespace MessengerApi.Persistence.Models
{
    public class ErrorLogger
    {
        public static void Log(Exception ex, string UserName)
        {

            List<ErrorsLog> Logs = new List<ErrorsLog>();
            ErrorsLog EX = new ErrorsLog();
            EX.Message = ex.Message;
            EX.Source = ex.Source;
            EX.StackTrace = ex.StackTrace;
            EX.TargetSite = ex.TargetSite.ToString();
            EX.Time = DateTime.UtcNow;
            EX.UserName = UserName;
            Logs.Add(EX);
            Exception inner = ex.InnerException;


            while (inner != null)
            {
                EX.Message = ex.Message;
                EX.Source = ex.Source;
                EX.StackTrace = ex.StackTrace;
                EX.TargetSite = ex.TargetSite.ToString();
                EX.Time = DateTime.UtcNow;
                EX.UserName = UserName;
                Logs.Add(EX);
                inner = inner.InnerException;
            }

            ApplicationDbContext db = new ApplicationDbContext();
            db.LogFile.AddRange(Logs);
            db.SaveChanges();
        }
    }
}