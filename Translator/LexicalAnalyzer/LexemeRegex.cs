using System;

namespace Translator.LexicalAnalyzer
{
    public class LexemeRegex
    {
        public static readonly string SplitPattern = @"\b[\w_][\w_]*|" +
                                                     @"[<>=;]|" +
                                                     @":=|" +
                                                     @"#.*$|" +
                                                     @"[^ #<>=;:_\w]*|";

        public static readonly string InvalidPattern = @"[^ #<>=:;_a-zA-Z0-9]";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lexeme"></param>
        /// <returns>Returns regex to determine invalid chars</returns>
        public static String GetRegexByLexeme(Lexemes lexeme)
        {
            switch (lexeme)
            {
                case Lexemes.Id:
                    return @"\b\d.*";
                case Lexemes.RomanNum:
                    return @"[^IVXLCDM]";
                default:
                    return "";
            }
        }
    }
}