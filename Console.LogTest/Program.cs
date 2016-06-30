using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jaap.Common.Mongodb;
using MongoDB.Bson;
using Console = System.Console;

namespace ConsoleApp.LogTest
{
    class Program
    {
        static void Main(string[] args)
        {

            for (int i = 0; i < 1000000; i++)
            {
                if (i%10000 == 0)
                {
                    Thread.Sleep(500);
                    Console.WriteLine("have a rest.....");
                }
                else
                {
                    var guid = Guid.NewGuid();
                    Console.WriteLine("正在创建:"+guid + "\t" + i);
                    var obj = new {Id = i, Name = guid, Time = string.Format("{0:yyyy-MM-dd HH:mm:ss-ffff}", DateTime.Now)};
                    LogHelper.LogToFile("正在创建:"+obj.ToJson());
                    LogHelper.Info(obj);
                }
            }

            Console.ReadKey();
        }
    }

    public class LogHelper
    {
        private static readonly Queue<Logger> QueueList;

        private static readonly object QueLock;
        private static readonly object fileLock;
        private static readonly object dbLock;

        static LogHelper()
        {
            QueueList=new Queue<Logger>();
            QueLock  = new object();
            fileLock = new object();
            dbLock = new object();
            ThreadPool.QueueUserWorkItem(Log, null);
        }

       public static void LogToFile(string message)
        {
            lock (fileLock)
            {
                if (!Directory.Exists("D:\\logs"))
                {
                    Directory.CreateDirectory("D:\\logs");
                }
                if (!File.Exists("D:\\logs\\log_mongo.txt"))
                {
                    using (FileStream fs = File.Create("D:\\log_mongo.txt"))
                    {
                        fs.Flush();
                    }
                }
                else
                {
                    FileInfo f = new FileInfo("D:\\logs\\log_mongo.txt");
                    if (f.Length -(f.Length-(1024*3))== 1024*3)
                    {
                        f.CopyTo(string.Format("D:\\logs\\log_mongo_{0:yy-MM-dd-hh-mm-ss-fff}.txt", DateTime.Now));
                    }
                }

                using (FileStream file = new FileStream("D:\\logs\\log_mongo.txt", FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        sw.WriteLine(message);
                        sw.Flush();
                    }
                }
            }
        }

        public static void Info(object content)
        {
            Log(content,Level.Info);
        }

        public static void Debug(object content)
        {
            Log(content,Level.Debug);
        }

        private static void LogToQueue(object state)
        {
            lock (QueLock)
            {
                var logger = state as Logger;
                QueueList.Enqueue(logger);
            }
        }

        private static void Log(object content,Level level)
        {
            var logger = new Logger {Content = content.ToJson(), Level = level};
            ThreadPool.QueueUserWorkItem(LogToQueue, logger);
        }

        private static void Log(object state)
        {
            while (true)
            {
                try
                {
                    if (QueueList.Any())
                    {
                        lock (QueLock)
                        {
                            var logger = QueueList.Dequeue();
                            ThreadPool.QueueUserWorkItem(LogToDb, logger);
                        }
                    }
                    else
                    {
                        LogToFile("队列暂时空了....");
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                    Thread.Sleep(1000000);
                }
               
            }
        }

        private static void LogToDb(object state)
        {
            var logger = state as Logger;
            LogToFile("正在记录:" + logger.ToJson());
            lock (dbLock)
            {
                var db = new LogDbContent();
                db.Add(logger);
            }
        }
    }

    public class Logger
    {
        public Level Level { get; set; }
        public string Content { get; set; }
    }

    public enum Level
    {
        Info=1,
        Debug=2,
        Warn=3,
        Error=4,
        Fatal=5
    }

    public class LogInfo
    {
        public ObjectId Id { get; set; }
        public string Message { get; set; }
    }

    public class LogDbContent : MongoBaseDbContext
    {
        public LogDbContent(string connectionStringName = "mongolog") : base(connectionStringName) { }
    }
}
