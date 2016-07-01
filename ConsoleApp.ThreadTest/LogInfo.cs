using System;
using MongoDB.Bson;

namespace ConsoleApp.ThreadTest
{
    public class LogInfo
    {
        public ObjectId Id { get; set; }
        public LogInfo()
        {
            LogTime = DateTime.Now;
        }

        public string Message { get; set; }
        public int Lid { get; set; }

        public DateTime LogTime { get; set; }
    }
}