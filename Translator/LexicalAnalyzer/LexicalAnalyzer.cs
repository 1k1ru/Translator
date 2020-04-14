using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Translator.LexicalAnalyzer
{
    public class LexicalAnalyzer
    {
        public List<Lexeme> LexTable { get; }
        public Dictionary<string, int> IdTable { get; }
        
        private States state;
        
        public LexicalAnalyzer(string code)
        {
            LexTable = new List<Lexeme>();
            IdTable = new Dictionary<string, int>();
            Analyze(code);
        }

        private void Analyze(string source)
        {
            source += '\n';
            state = States.Start;
            int idCounter = 0;
            int lineCounter = 1;
            Lexeme lexeme = new Lexeme();

            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                switch (state)
                {
                    case States.Start:
                        lexeme = new Lexeme();
                        lexeme.Name = c.ToString();
                        
                        switch (c)
                        {
                            case '\n':
                                lineCounter++;
                                break;
                            case ' ': case '\t': case '\r': case '\0':
                                break;
                            case '#':
                                state = States.Comment;
                                break;
                            case ';':
                                lexeme.LexemeType = LexemeTypes.Separator;
                                LexTable.Add(lexeme);
                                break;
                            case ':':
                                state = States.Assignment;
                                break;
                            case '<': case '>': case '=':
                                lexeme.LexemeType = LexemeTypes.Comparison;
                                LexTable.Add(lexeme);
                                break;
                            default:
                                state = Regex.IsMatch(c.ToString(), @"[a-zA-Z_]") 
                                    ? States.Word 
                                    : States.Error;
                                break;
                        }
                        break;
                    
                    case States.Word:
                        if (Regex.IsMatch(c.ToString(), @"[a-zA-Z0-9_]"))
                        {
                            lexeme.Name += c;
                        }
                        else
                        {
                            if (lexeme.Name == "if" || lexeme.Name == "then" || lexeme.Name == "else")
                            {
                                lexeme.LexemeType = LexemeTypes.Condition;
                            }
                            else if (Regex.IsMatch(lexeme.Name, @"[IVXLCDM]"))
                            {
                                lexeme.LexemeType = LexemeTypes.RomanNum;
                            }
                            else
                            {
                                lexeme.LexemeType = LexemeTypes.Id;
                                try
                                {
                                    IdTable.Add(lexeme.Name, idCounter);
                                    idCounter++;
                                }
                                catch (Exception ignore)
                                {
                                    //ignored
                                }
                                
                            }
                            
                            LexTable.Add(lexeme);
                            i--;
                            state = States.Start;
                        }
                        break;
                    
                    case States.Assignment:
                        if (c == '=')
                        {
                            lexeme.LexemeType = LexemeTypes.Assignment;
                            lexeme.Name += c;
                            LexTable.Add(lexeme);
                            state = States.Start;
                        }
                        else
                        {
                            state = States.Error;
                        }
                        break;
                    
                    case States.Comment:
                        if (c != '\n')
                        {
                            lexeme.Name += c;
                        }
                        else
                        {
                            lexeme.LexemeType = LexemeTypes.Comment;
                            LexTable.Add(lexeme);
                            state = States.Start;
                            lineCounter++;
                        }
                        break;
                    
                    case States.Error:
                        throw new Exception($"line {lineCounter}: invalid input char \'{c}\'");
                }
            }
        }
    }
}