using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Translator.LexicalAnalyzer.FA
{
    public class LexicalAnalyzerFA
    {
        public List<LexTableNode> LexTable { get; private set; }
        public Dictionary<string, int> IdTable { get; private set; }
        
        private StatesFA state;
        
        public LexicalAnalyzerFA(string code)
        {
            LexTable = new List<LexTableNode>();
            IdTable = new Dictionary<string, int>();
            Analyze(code);
        }

        private void Analyze(string code)
        {
            code += '\n';
            state = StatesFA.Start;
            int idCounter = 0;
            LexTableNode lexeme = new LexTableNode();
            
            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];
                switch (state)
                {
                    case StatesFA.Start:
                        lexeme = new LexTableNode();
                        lexeme.Name = c.ToString();
                        
                        switch (c)
                        {
                            case ' ': case '\t': case '\n': case '\r': case '\0':
                                break;
                            case '#':
                                state = StatesFA.Comment;
                                break;
                            case ';':
                                lexeme.Lexeme = Lexemes.Separator;
                                LexTable.Add(lexeme);
                                break;
                            case ':':
                                state = StatesFA.Assignment;
                                break;
                            case '<': case '>': case '=':
                                lexeme.Lexeme = Lexemes.Comparison;
                                LexTable.Add(lexeme);
                                break;
                            default:
                                if (Regex.Matches(c.ToString(), @"[^a-zA-Z_]").Count == 0)
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
                        if (Regex.Matches(c.ToString(), @"[^a-zA-Z0-9_]").Count == 0)
                        {
                            lexeme.Name += c;
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
                                try
                                {
                                    IdTable.Add(lexeme.Name, idCounter);
                                    idCounter++;
                                }
                                catch (Exception ignored)
                                {
                                    
                                }
                                
                            }
                            
                            LexTable.Add(lexeme);
                            i--;
                            state = StatesFA.Start;
                        }
                        break;
                    
                    case StatesFA.Assignment:
                        if (c == '=')
                        {
                            lexeme.Lexeme = Lexemes.Assignment;
                            lexeme.Name += c;
                            LexTable.Add(lexeme);
                            state = StatesFA.Start;
                        }
                        else
                        {
                            state = StatesFA.Error;
                        }
                        break;
                    
                    case StatesFA.Comment:
                        if (c != '\n')
                        {
                            lexeme.Name += c;
                        }
                        else
                        {
                            lexeme.Lexeme = Lexemes.Comment;
                            LexTable.Add(lexeme);
                            state = StatesFA.Start;
                        }
                        break;
                    
                    case StatesFA.Error:
                        throw new Exception("invalid input char \'" + c + "\'");
                }
            }
        }
    }
}