using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Translator.LexicalAnalyzer;
using Translator.LexicalAnalyzer.FA;

namespace Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "input.txt";
            if (args.Length != 0)
            {
                path = args[0];
            }

            List<LexTableNode> table = new List<LexTableNode>();
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                StringBuilder sb = new StringBuilder();
                while (fs.Read(b,0,b.Length) > 0)
                {
                    sb.Append(temp.GetString(b));
                }
                
                table = LexicalAnalyzerFA.Analyze(sb.ToString());
                
                foreach (var lexeme in table)
                {
                    Console.WriteLine(lexeme.Name + " \t\t: " + lexeme.Lexeme);
                }
            }
        }
    }
}