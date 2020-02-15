using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Translator.LexicalAnalyzer
{
    public class LexicalAnalyzer
    {
        private static readonly Dictionary<Lexemes, List<string>> Reserved = new Dictionary<Lexemes, List<string>>()
        {
            { Lexemes.Condition, new List<string>() { "if", "then", "else" } },
            { Lexemes.Comparison, new List<string>() { "<", ">", "=" } },
            { Lexemes.Separator, new List<string>() { ";" } },
            { Lexemes.Assignment, new List<string>() { ":=" } }
        };

        public static List<LexTableNode> Analyze(string ex)
        {
            MatchCollection matches = Regex.Matches(ex, LexemeRegex.SplitPattern);
            List<string> validLexemes = new List<string>();
            List<string> invalidLexemes = new List<string>();

            foreach (Match match in matches)
            {
                string m = match.ToString();
                
                if (m.Length == 0)
                    continue;

                if (m[0] == '#' ||
                    Regex.Matches(m, LexemeRegex.InvalidPattern).Count == 0 &&
                    Regex.Matches(m, LexemeRegex.GetRegexByLexeme(Lexemes.Id)).Count == 0)
                {
                    validLexemes.Add(m);
                }
                else
                {
                    invalidLexemes.Add(m);
                }
            }

            if (invalidLexemes.Count != 0)
            {
                string m = "Invalid input ex. Invalid chars and lexemes:\n";
                foreach (var s in invalidLexemes)
                {
                    m += "\"" + s + "\"\n";
                }
                throw new Exception(m);
            }

            List<LexTableNode> lexTable = new List<LexTableNode>();

            foreach (var lexeme in validLexemes)
            {
                LexTableNode tableNode = new LexTableNode();
                tableNode.Name = lexeme;

                bool reservedFound = false;
                foreach (var pair in Reserved)
                {
                    if (pair.Value.Contains(lexeme))
                    {
                        tableNode.Lexeme = pair.Key;
                        reservedFound = true;
                        break;
                    }
                }
                
                if (!reservedFound)
                    if (Regex.Matches(lexeme, LexemeRegex.GetRegexByLexeme(Lexemes.RomanNum)).Count == 0)
                        tableNode.Lexeme = Lexemes.RomanNum;
                    else if (lexeme[0] == '#')
                        tableNode.Lexeme = Lexemes.Comment;
                    else
                        tableNode.Lexeme = Lexemes.Id;

                lexTable.Add(tableNode);
            }

            return lexTable;
        }
    }
}