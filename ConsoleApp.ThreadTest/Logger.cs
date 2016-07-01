using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using MongoDB.Bson;

namespace ConsoleApp.ThreadTest
{
    public class Logger
    {
        private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
        private readonly Queue<LogInfo> _queue;
        private static readonly object _fileLock = new object();

        public Logger()
        {
            _queue = new Queue<LogInfo>();
        }

        private void AddLogInfo(LogInfo log)
        {
            lockSlim.EnterWriteLock();
            try
            {
                _queue.Enqueue(log);
                
                ThreadPool.QueueUserWorkItem(Write,null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        private LogInfo GetLogInfo()
        {

            lockSlim.EnterReadLock();
            try
            {
                Console.WriteLine("read item from queue...");
                return _queue.Any() ? _queue.Dequeue() : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public void Info(LogInfo logInfo)
        {
            AddLogInfo(logInfo);
        }

        private void Write(object state)
        {
            Console.WriteLine("Method Write....");
            while (true)
            {
                var logInfo = GetLogInfo();
                if (logInfo != null)
                    //WriteToMongo(logInfo);
                    WirteToFile(logInfo);
                else
                {
                    return;
                }
            }
        }


        private void WriteToMongo(LogInfo logInfo)
        {
            var db = new LogDbContext();
            //lock (_fileLock)
            //{
                Console.WriteLine("write to mongo:{0}",logInfo.Lid);
               
                db.Add(logInfo);
            //}
        }

        private void WirteToFile(LogInfo logInfo   )
        {
            //Console.WriteLine("start write to file....");
            lock (_fileLock)
            {
                string dirPath = "D:\\logs";
                string fileName = "test_logs.txt";
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                string filepath = dirPath + "\\" + fileName;
                if (!File.Exists(dirPath + "\\" + fileName))
                {
                    using (FileStream fs = File.Create(filepath))
                    {
                        fs.Flush();
                    }
                }
                using (FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        string message = string.Format("{0}--{1:yy-MM-dd hh:mm:ss-fff}--{2}", Thread.CurrentThread.Name,
                            DateTime.Now, logInfo.ToJson());
                        Console.WriteLine(message);
                        sw.WriteLine(message);
                    }
                }
            }
            Console.WriteLine("end write to file.......");
        }
    }
}