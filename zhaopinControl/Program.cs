using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace zhaopinControl
{
    class Program
    {
        /// <summary>
        /// http://sou.zhaopin.com/jobs/searchresult.ashx?jl=%E9%95%BF%E6%B2%99&sm=0&p=1
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            //todojaap:完成页面解析
            var urlList = new List<string>();

            for (int i = 4; i < 6; i++)
            {
                var baseUrl =
                    string.Format("http://sou.zhaopin.com/jobs/searchresult.ashx?jl=%E9%95%BF%E6%B2%99&sm=0&p={0}", i);
                var content = WebRequest.Create(baseUrl).Download(Encoding.UTF8);
                var pattern = "<a.*?style=\"font-weight: bold\".*?par=\"(?<params>.*?)\".*?href=\"(?<url>.*?)\".*?>.*?</a>";
                var reg = new Regex(pattern, RegexOptions.Singleline);
                foreach (Match mc in reg.Resolve(content))
                {
                    urlList.Add(mc.Groups["url"].Value + "?" + mc.Groups["params"]);
                }
            }

            foreach (var url in urlList.Distinct())
            {
                Console.WriteLine("------------------------------");
                var content = WebRequest.Create(url).Download(Encoding.UTF8);
                var pattern_gwmc = "<div.*?class=\"inner-left fl\">.*?<h1>(?<gwmc>.*?)</h1>";
                var pattern_zwyx = "<li><span>职位月薪：</span><strong>(?<zwyx>.*?)</strong></li>";
                var pattern_address = "<li><span>工作地点：</span><strong>(?<address>.*?)</strong></li>";
                //var pattern_company = "<h2>.*?<a href=\"(?<comUrl>http://company.*?)\" onclick=\".*?\">(?<comName>.*?)</a>.*?</h2>";
               
                Console.WriteLine(new Regex(pattern_gwmc, RegexOptions.Singleline).Match(content).Groups["gwmc"].Value);
                Console.WriteLine(new Regex(pattern_zwyx, RegexOptions.Singleline).Match(content).Groups["zwyx"].Value);
                //Console.WriteLine(new Regex(pattern_address, RegexOptions.Singleline).Match(content).Groups["address"].Value);
                var address = new Regex(pattern_address, RegexOptions.Singleline)
                    .Match(content)
                    .Groups["address"].Value
                    .ReplaceByReg("<a.*?>", "").ReplaceByReg("</a.*?>", "");

                Console.WriteLine(address);
            }

        }
    }
}
