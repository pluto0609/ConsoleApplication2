using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp.ReplaceContent
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Clipboard.Clear();
            var path = @"D:\test.html";
            var content = string.Empty;
            using (FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    content = sr.ReadToEnd();
                }
            }

            Clipboard.GetText(TextDataFormat.Text);
            //content = Regex.Replace(content, "<font.*?>|</font.*?>|<[/]{0,1}u.*?>|&nbsp;", "");
            //content = Regex.Replace(content, "<br.*?>", "\n");

            //content = Regex.Replace(content, "<table.*?>|</table.*?>", "#t#");
            //content = Regex.Replace(content, "<tr.*?>|</tr.*?>", "#r#");
            //content = Regex.Replace(content, "<td.*?>|</td.*?>", "#d#");

            content = Regex.Replace(content, "<head.*?>.*?</head>|<script.*?>.*?</script>|&nbsp;", "");

            Clipboard.SetText(content);
            Console.WriteLine(content);
        }
    }
}
