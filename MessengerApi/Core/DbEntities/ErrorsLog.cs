using System;

namespace MessengerApi.Core.DbEntities
{
    public class ErrorsLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string TargetSite { get; set; }
        public string UserName { get; set; }
        public DateTime Time { get; set; }
    }
}