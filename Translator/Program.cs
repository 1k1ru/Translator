using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Translator.LexicalAnalyzer;

namespace Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            //String input = "if a = _a2 then b1_:= _b2; else c1:=XVI; //jk";

            string input = Console.ReadLine();
            
            List<LexTableNode> table = LexicalAnalyzer.LexicalAnalyzer.Analyze(input);
            
            foreach (var lexeme in table)
            {
                Console.WriteLine(lexeme.Name + " \t\t: " + lexeme.Lexeme);
            }
        }
    }
}