using System;
using System.Text.RegularExpressions;

namespace zhaopinControl
{
    public static class RegexExtension
    {
        public static MatchCollection Resolve(this Regex regex, string context)
        {
            if (string.IsNullOrEmpty(context)) return null;
            MatchCollection mclistMenu = regex.Matches(context);
            if (mclistMenu.Count==0)
            {
                return null;
            }
            return mclistMenu;
        }

        /// <summary>
        /// 正则表达式替换
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="replayment">要替换成的字符串</param>
        /// <returns></returns>
        public static string ReplaceByReg(this string input, string pattern, string replayment)
        {
            string val = Regex.Replace(input, pattern, replayment);
            //Console.WriteLine(val);
            return val;
        }
    }
}