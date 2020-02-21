using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Translator.LexicalAnalyzer.FA
{
    public class LexicalAnalyzerFA
    {
        private static StatesFA state;

        public static List<LexTableNode> Analyze(string ex)
        {
            ex += '\n';
            state = StatesFA.Start;
            List<LexTableNode> table = new List<LexTableNode>();
            LexTableNode lexeme = new LexTableNode();
            
            for (int i = 0; i < ex.Length; i++)
            {
                switch (state)
                {
                    case StatesFA.Start:
                        lexeme = new LexTableNode();
                        lexeme.Name = ex[i].ToString();
                        
                        switch (ex[i])
                        {
                            case ' ': case '\t': case '\n':
                                break;
                            case '#':
                                state = StatesFA.Comment;
                                break;
                            case ';':
                                lexeme.Lexeme = Lexemes.Separator;
                                table.Add(lexeme);
                                break;
                            case ':':
                                state = StatesFA.Assignment;
                                break;
                            case '<': case '>': case '=':
                                lexeme.Lexeme = Lexemes.Comparison;
                                table.Add(lexeme);
                                break;
                            default:
                                if (Regex.Matches(ex[i].ToString(), @"[^a-zA-Z_]").Count == 0)
                                {
                                    state = StatesFA.Word;
                                }
                                else
                                {
                                    state = StatesFA.Error;
                                }
                                break;
                        }
                        break;
                    
                    case StatesFA.Word:
                        if (Regex.Matches(ex[i].ToString(), @"[^a-zA-Z0-9_]").Count == 0)
                        {
                            lexeme.Name += ex[i];
                        }
                        else
                        {
                            if (lexeme.Name == "if" || lexeme.Name == "then" || lexeme.Name == "else")
                            {
                                lexeme.Lexeme = Lexemes.Condition;
                            }
                            else if (Regex.Matches(lexeme.Name, @"[^IVXLCDM]").Count == 0)
                            {
                                lexeme.Lexeme = Lexemes.RomanNum;
                            }
                            else
                            {
                                lexeme.Lexeme = Lexemes.Id;
                            }
                            
                            table.Add(lexeme);
                            i--;
                            state = StatesFA.Start;
                        }
                        break;
                    
                    case StatesFA.Assignment:
                        if (ex[i] == '=')
                        {
                            lexeme.Lexeme = Lexemes.Assignment;
                            lexeme.Name += ex[i];
                            table.Add(lexeme);
                            state = StatesFA.Start;
                        }
                        else
                        {
                            state = StatesFA.Error;
                        }
                        break;
                    
                    case StatesFA.Comment:
                        if (ex[i] != '\n')
                        {
                            lexeme.Name += ex[i];
                        }
                        else
                        {
                            lexeme.Lexeme = Lexemes.Comment;
                            table.Add(lexeme);
                            state = StatesFA.Start;
                        }
                        break;
                    
                    case StatesFA.Error:
                        throw new Exception("invalid input char");
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return table;
        }
    }
}