using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Translator.LexicalAnalyzer;
using Translator.SyntacticAnalyzer;

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

            List<Lexeme> table = new List<Lexeme>();
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                StringBuilder sb = new StringBuilder();
                while (fs.Read(b,0,b.Length) > 0)
                {
                    sb.Append(temp.GetString(b));
                }

                LexicalAnalyzer.LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer.LexicalAnalyzer(sb.ToString());

                Console.WriteLine("Lexeme table:");
                foreach (var lexeme in lexicalAnalyzer.LexTable)
                {
                    Console.WriteLine(lexeme.Name + " \t\t: " + lexeme.LexemeType);
                }
                
                Console.WriteLine("\nId table:");
                foreach (var id in lexicalAnalyzer.IdTable)
                {
                    Console.WriteLine(id.Key + " \t\t: " + id.Value);
                }

                SyntacticAnalyzer.SyntacticAnalyzer syntacticAnalyzer 
                    = new SyntacticAnalyzer.SyntacticAnalyzer(lexicalAnalyzer.LexTable, lexicalAnalyzer.IdTable);

                Console.WriteLine("\nAST:");
                syntacticAnalyzer.Print();
            }
        }
    }
}