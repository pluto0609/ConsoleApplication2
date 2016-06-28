using System;
using System.IO;
using System.Net;
using System.Text;

namespace zhaopinControl
{
    public static class WebRequestExtension
    {
        public static string Download(this WebRequest request,Encoding encoding)
        {
            var response = request.GetResponse() as HttpWebResponse;

            if (response == null || response.StatusCode != HttpStatusCode.OK) return string.Empty;

            //foreach (var key in response.Headers.AllKeys)
            //{
            //    Console.WriteLine("{0}\t{1}", key, response.Headers[key]);
                
            //}
            //Console.WriteLine("-----------------");
            //Console.WriteLine(response.Headers["content-type"]);

            var stream = response.GetResponseStream();

            if (stream == null) return string.Empty;

            var reader = new StreamReader(stream, encoding);
            var context = reader.ReadToEnd();
            return context;
        }

        public static WebRequest PrintInfo(this WebRequest request)
        {
            foreach (var key in request.Headers.AllKeys)
            {
                Console.WriteLine("{0}\t{1}", key, request.Headers[key]);
            }
            Console.WriteLine("-------------------------");
            return request;
        }
    }
}