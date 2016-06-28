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
        /// ������ʽ�滻
        /// </summary>
        /// <param name="input">Դ�ַ���</param>
        /// <param name="pattern">������ʽ</param>
        /// <param name="replayment">Ҫ�滻�ɵ��ַ���</param>
        /// <returns></returns>
        public static string ReplaceByReg(this string input, string pattern, string replayment)
        {
            string val = Regex.Replace(input, pattern, replayment);
            //Console.WriteLine(val);
            return val;
        }
    }
}