using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;
using Jaap.SpiderFramework;

namespace ZhaoBiaoControl
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var req = WebRequest.Create("http://www.ccgp-hunan.gov.cn:8080/portal/protalAction!getNoticeList.action").SetMethod(RequestMethod.Post);
            var r = req as HttpWebRequest;
            r.ContentType = "application/x-www-form-urlencoded";
            r.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            
            string paramsBody = "nType=dealNotices&pType=&prcmPrjName=&prcmItemCode=&prcmOrgName=&startDate=&endDate=&page=1&pageSize=18";
            byte[] btBodys = Encoding.UTF8.GetBytes(paramsBody);
            r.ContentLength = btBodys.Length;
            r.GetRequestStream().Write(btBodys,0,btBodys.Length);

            
            var response = r.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var content = sr.ReadToEnd();

            var urlList = new List<string>();

            //
            var urlFormat = "http://www.ccgp-hunan.gov.cn:8080/portal/protalAction!viewNoticeContent.action?noticeId={0}&area_id=";

            int index = 0;
            foreach (Match match in Regex.Matches(content, "\"NOTICE_ID\":(?<id>\\d*?),"))
            {
                if (match.Success)
                {
                    Console.WriteLine(index + "\t" + match.Groups["id"]);
                    urlList.Add(string.Format(urlFormat,match.Groups["id"]));
                    index++;
                }
            }

            foreach (var s in urlList)
            {
                Console.WriteLine(s);
            }
            
            Clipboard.SetText(content);
            Console.WriteLine("end");

            //Console.WriteLine(content);


            //Json.Decode<>()

            //foreach (var key in r.Headers.AllKeys)
            //{
            //    Console.WriteLine(req.Headers[key]);
            //}

            Console.Read();
        }
    }
}
