using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jaap.SpiderFramework
{
    public static class WebRequestExtension
    {
        /// <summary>
        /// 添加头部信息
        /// </summary>
        /// <param name="request">请求<see cref="System.Net.WebRequest"/></param>
        /// <param name="keyName">头部值名称</param>
        /// <param name="keyValue">值</param>
        /// <returns></returns>
        public static WebRequest AddHeader(this WebRequest request, string keyName, string keyValue)
        {
            request.Headers.Add(keyName,keyValue);
            return request;
        }

        /// <summary>
        /// 设置请求方式,如Post或Get等
        /// </summary>
        /// <param name="request"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static WebRequest SetMethod(this WebRequest request, RequestMethod method= RequestMethod.Get)
        {
            if (method == RequestMethod.Post)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            request.Method = method.ToString().ToUpper();
            return request;
        }
    }
}
