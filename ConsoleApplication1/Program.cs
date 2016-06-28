using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        private static Object _objA = new Object();
        private static Object _objB = new Object();

        private static void LockA()
        {
            if (Monitor.TryEnter(_objA, 1000))
            {
                Thread.Sleep(1000);
                if (Monitor.TryEnter(_objB,2000))
                {
                    Monitor.Exit(_objB);
                }
                else
                {
                    Console.WriteLine("LockB timeout");
                }
                Monitor.Exit(_objA);
            }
            Console.WriteLine("LockA");
        }

        private static void LockB()
        {
            if (Monitor.TryEnter(_objB,2000))
            {
                Thread.Sleep(2000);
                if (Monitor.TryEnter(_objA,1000))
                {
                    Monitor.Exit(_objA);
                }
                else
                {
                    Console.WriteLine("LockA timeout");
                }
                Monitor.Exit(_objB);
            }
            Console.WriteLine("LockB");
        }

        static void Main(string[] args)
        {
            #region 

            //Thread ta = new Thread(LockA);
            //Thread tb = new Thread(LockB);
            //ta.Start();
            //tb.Start();
            //Thread.Sleep(4000);
            //Console.WriteLine("Thread end...");

            #endregion

            var dir = new DirectoryInfo(@"D:\宁小闲御神录_3\");

            Dictionary<int,string> dic = new Dictionary<int, string>();
            foreach (var fileInfo in dir.GetFiles())
            {
                string filename = fileInfo.Name;
                string pattern = @"第(?<index>\d*?)章.*?";
                Regex reg = new Regex(pattern);
                var mc = reg.Match(filename);
                if (mc.Success)
                {
                    var index = int.Parse(mc.Groups["index"].Value);
                    if(!dic.Keys.Contains(index))
                    dic.Add(index,filename);
                    else
                    {
                        Console.WriteLine("{0}\t{1}",index,filename);
                        var v = Console.ReadKey();
                    }
                }
                    //Console.WriteLine(mc.Groups["index"] + "\t" + filename);
                //Console.WriteLine(fileInfo.Name);
            }


            foreach (var item in dic.OrderBy(g=>g.Key))
            {
                //Console.WriteLine("{0}\t{1}", item.Key, item.Value);
                Console.WriteLine("正在读取:{0}", item.Value);
                StringBuilder sb = new StringBuilder();
                sb.Append("------------------------------------");
                //if(!File.Exists(item.Value)) continue;

                var fullPath = dir + item.Value;

                FileInfo f = new FileInfo(fullPath);
                if(!f.Exists) continue;

                sb.Append(f.Name);
                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string page = sr.ReadToEnd();
                        sb.Append(page);
                    }
                }
                sb.Append("------------------------------------");
                Console.WriteLine("正在写入:{0}", fullPath);
                AppendToFile(sb.ToString());
            }
        }

        private static void AppendToFile(string content)
        {
            var savePath = @"D:\宁小闲御神录_3\total.txt";
            //FileInfo f = new FileInfo(savePath);
            //if (!f.Exists) f.Create();
            
            
            using (FileStream fs = new FileStream(savePath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    //Console.WriteLine(正在写入);
                    sw.Write(content.Replace("/",""));
                    sw.Flush();
                }
            }
        }
    }
}
