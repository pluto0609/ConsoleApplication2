using System;
using System.IO;
using System.Threading;

namespace IOThreadTest
{
    class Program
    {
        private static object _threadLock = new object();

        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)
            {
                ThreadPool.QueueUserWorkItem(Write, @"D:\io.txt");
            }
            Console.WriteLine("end.....");
            Console.ReadKey();
        }

        private static void Write(object state)
        {
            var filePath =(string)state;

            lock (_threadLock)
            {
                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        fs.Flush();
                        fs.Close();
                    }
                }
            
                using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        var num = Guid.NewGuid();
                        Console.WriteLine("开始写入:{0}", num);
                        sw.Write("{0}\n", num);
                        sw.Flush();
                    }
                }
            }
        }
    }
}
