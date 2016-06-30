using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleReader
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("文件读取启动,请选择要读取的文件.....");
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var filePath = ofd.FileName;
            var content = string.Empty;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    content = sr.ReadToEnd();
                }
            }

            if(string.IsNullOrEmpty(content)) return;

            var lines = new List<string>();
            foreach (Match match in Regex.Matches(content, "(?<line>.*?)\\n"))
            {
                lines.Add(match.Groups["line"].Value);
            }

            if (lines.Count == 0) return;

            var index = 0;
            var length = 5;
            while (index+length<lines.Count)
            {
                Console.Clear();

                for (int i = index; i < index+ length; i++)
                {
                    Console.WriteLine(lines[i]);
                }


                var nextCommandKey = Console.ReadKey(false);
                switch (nextCommandKey.Key)
                {
                    case  ConsoleKey.PageDown:
                        var chapterPattern = ".*?第\\d*?章.*?";
                        Regex reg = new Regex(chapterPattern);
                        for (int j = index; j < lines.Count; j++)
                        {
                            if (reg.IsMatch(lines[j]))
                            {
                                index = j;
                                break;
                            }
                        }
                        break;
                    case ConsoleKey.Backspace:
                        index -= 2*length;
                        if (index < 0) index = 0;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.N:
                    case ConsoleKey.Enter:
                        index += length;
                        break;
                    case ConsoleKey.Escape:
                        return;
                    default:
                        continue;
                }
            }

        }
    }
}
