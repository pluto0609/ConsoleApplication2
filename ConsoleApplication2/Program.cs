using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {

            var context = DownloadPage("http://www.quledu.com/wcxs-41566/");

            var urlList = new Dictionary<string, string>();

            //string path="";
            //SaveContent(path, context);

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
                    var content = DownloadPage("http://www.quledu.com" + url);
                    string contentPatten = "<div.*?id=\"htmlContent\".*?>(?<content>.*?)</div>";
                    var mcList = Resolve(contentPatten, content);
                    if (mcList == null) continue;

                    foreach (Match mc in mcList)
                    {
                        if (mc.Success)
                        {
                            string path = string.Format(@"{0}\{1}.txt", @"D:\宁小闲御神录_2\", urlList[url]);
                            Console.WriteLine("正在下载[{0}]\t保存路径为:[{1}]", url, path);
                            SaveContent(path,
                                mc.Groups["content"].Value);
                        }
                    }
                }
        }

        private static MatchCollection Resolve( string menuPattern,string context)
        {
            //menuPattern = "<li.*?>.*?<a.*?href=\"(?<url>.*?)\".*?>(?<title>.*?)</a.*?>.*?</li.*?>";
            if (string.IsNullOrEmpty(context)) return null;
            Regex reg_menu = new Regex(menuPattern, RegexOptions.Singleline);
            MatchCollection mclistMenu = reg_menu.Matches(context);
            return mclistMenu;
        }


        private static void SaveContent(string path, string context)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var sw = new StreamWriter(fs);
                sw.Write(context);
                sw.Flush();
                fs.Flush();
            }
        }

        private static string DownloadPage(string url)
        {
            var request = WebRequest.Create(url);
            //var user_agent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";
            //request.Headers.Set("User-Agent",user_agent);
            
            var response = request.GetResponse() as HttpWebResponse;

            //if(response.)
            //response as HttpWebResponse

            if (response.StatusCode != HttpStatusCode.OK) return string.Empty;

            var stream = response.GetResponseStream();

            if (stream == null) return string.Empty;

            var reader = new StreamReader(stream, Encoding.Default);
            var context = reader.ReadToEnd();
            return context;
        }

    }
}
