using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ConsoleApp.ThreadTest
{
    class Program
    {
        private static readonly object dbLock = new object();

        static void Main(string[] args)
        {
            #region 

//Console.WriteLine("start......v");


            //LogDbContext db = new LogDbContext();


            //for (int i = 0; i < 100; i++)
            //{
            //    db.Add(new LogInfo(){Lid=i,Message = Guid.NewGuid().ToString()});
            //}

            //LogDbContext db = new LogDbContext();
            //db.Add(new LogInfo(){Lid=1111111,})

            //var list = new List<Logger>();

            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new Logger());
            //}

            //var index = 0;

            //foreach (var logger in list)
            //{
            //    for (int i = 0; i < 10000; i++)
            //    {
            //        logger.Info(new LogInfo { Lid = index, Message = Guid.NewGuid().ToString() });
            //        index++;
            //    }
            //}
            //Console.ReadKey();

            //var log = new Logger();
            //for (int i = 0; i < 1000; i++)
            //{
            //    log.Info(new LogInfo {Lid = i, Message = Guid.NewGuid().ToString()});
            //}

            //while (true)
            //{
            //    var key = Console.ReadLine();
            //    int id;
            //    if (!int.TryParse(key, out id))
            //    {
            //        id = 999991119;
            //    }
            //    log.Info(new LogInfo { Lid = id, Message = Guid.NewGuid().ToString() });
            //    Console.WriteLine("wait for next input.....");
            //}

            #endregion
            var logger = new MyLog();

            //ThreadPool.QueueUserWorkItem(x =>
            //{
            //    while (true)
            //    {
            //        var log = logger.GetNext();

            //        if (log != null)
            //        {
            //            var db = new LogDbContext();
            //            // log = MyLog.Queue.Dequeue();
            //            Console.WriteLine(log.ToJson());
            //            db.Add(log);
            //        }
            //        else
            //        {
            //            Console.WriteLine("have a rest.....");
            //            Thread.Sleep(500);
            //        }
            //    }
            //});

            //ThreadPool.QueueUserWorkItem(WriteToDb, logger);

            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine("--------" + i + "----------");
                logger.AddLog(new LogInfo { Lid = i, Message = Guid.NewGuid().ToString() });
            }
            WriteToDb(logger);
            Console.ReadLine();
        }

        private static void WriteToDb(object state)
        {
            var logger = state as MyLog;
            while (true)
            {
                var log = logger.GetNext();
                if (log != null)
                {
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        //lock (dbLock)
                        //{
                            var db = new LogDbContext();
                            Console.WriteLine(log.ToJson());
                            db.Add(log);
                        //}
                    });
                }
                else
                {
                    Console.WriteLine("have a rest.....");
                    Thread.Sleep(500);
                }
            }
        }
    }

    public class MyLog
    {
        public static readonly Queue<LogInfo> Queue;

        private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

        static MyLog()
        {
            Queue = new Queue<LogInfo>();
        }

        public LogInfo GetNext()
        {
            lockSlim.EnterReadLock();
            try
            {
                if (Queue.Count>0)
                {
                    return Queue.Dequeue();
                }
                //Console.WriteLine();
                return null;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public void AddLog(LogInfo log)
        {
            lockSlim.EnterWriteLock();
            try
            {
                Console.WriteLine("--------" + log.Lid + "----------");
                Queue.Enqueue(log);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }
    }
}
