using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleApplication2
{
    public class Target
    {
        public string Url { get; set; }
        public string Title { get; set; }
    }
    class Program
    {
        private delegate int NewTaskDelegate(int ms);

        static void Main(string[] args)
        {
            xiaoshuo();
            Console.ReadLine();
        }
        

        /// <summary>
        /// 下载小说
        /// </summary>
        private static void xiaoshuo()
        {
            var context = DownloadPage("http://www.quledu.com/wcxs-41566/");

            var urlList = new Dictionary<string, string>();
            
            string menuPattern = "<li.*?>.*?<a.*?href=\"(?<url>.*?)\".*?>(?<title>.*?)</a.*?>.*?</li.*?>";
            var mclistMenu = Resolve(menuPattern, context);

            if (mclistMenu != null)
                foreach (Match match in mclistMenu)
                {
                    if (match.Success)
                    {
                        urlList.Add(match.Groups["url"].Value, match.Groups["title"].Value);
                    }
                }
            Console.WriteLine(urlList.Count);

            if (urlList.Count > 0)
                foreach (var url in urlList.Keys)
                {
                    var title = urlList[url];
                    Target t = new Target {Title = title, Url = url};
                    ThreadPool.QueueUserWorkItem(GetUrlAndSave, t);
                }
        }

        private static void GetUrlAndSave(object target)
        {
            try
            {
                var t = target as Target;
                Dictionary<string, string> urlList;
                var content = DownloadPage("http://www.quledu.com" + t.Url);
                string contentPatten = "<div.*?id=\"htmlContent\".*?>(?<content>.*?)</div>";
                var mcList = Resolve(contentPatten, content);
                if (mcList == null) return;

                foreach (Match mc in mcList)
                {
                    if (mc.Success)
                    {
                        string path = string.Format(@"{0}\{1}.txt", @"D:\宁小闲御神录_3\", t.Title);
                        Console.WriteLine("正在下载[{0}]\t保存路径为:[{1}]", t.Url, path);
                        SaveContent(path,
                            mc.Groups["content"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 解析指定文本内容
        /// </summary>
        /// <param name="menuPattern"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static MatchCollection Resolve( string menuPattern,string context)
        {
            if (string.IsNullOrEmpty(context)) return null;
            Regex reg_menu = new Regex(menuPattern, RegexOptions.Singleline);
            MatchCollection mclistMenu = reg_menu.Matches(context);
            return mclistMenu;
        }


        private static string Replace(string pattern,string content)
        {
            Regex re = new Regex(pattern, RegexOptions.Singleline);
            var str = re.Replace(content, "");
            return str;
        }


        public static void SaveContent(string path, string context)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                context = Replace("[(&nbsp;)|(<br.*?>)|(<img.*?>)].*?", context);
                var sw = new StreamWriter(fs);
                sw.Write(context);
                sw.Flush();
                fs.Flush();
            }
        }

        private static string DownloadPage(string url)
        {
            var request = WebRequest.Create(url);
            
            var response = request.GetResponse() as HttpWebResponse;
            
            if (response.StatusCode != HttpStatusCode.OK) return string.Empty;

            var stream = response.GetResponseStream();

            if (stream == null) return string.Empty;

            var reader = new StreamReader(stream, Encoding.Default);
            var context = reader.ReadToEnd();
            return context;
        }

    }
}
