using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Translator.LexicalAnalyzer;

namespace Translator.SyntacticAnalyzer
{
    public class SyntacticAnalyzer
    {
        private List<Lexeme> lexTable;
        private Dictionary<string, int> idTable;

        private int index;

        private AstNode root;

        private Lexeme Current { get { return lexTable[index]; } }

        public SyntacticAnalyzer(List<Lexeme> lexTable, Dictionary<string, int> idTable)
        {
            this.lexTable = lexTable;
            this.idTable = idTable;

            DeleteComments();
            root = Analyze();
        }

        public void Print()
        {
            root.Print();
        }

        private void DeleteComments()
        {
            for (int i = 0; i < lexTable.Count; i++)
            {
                if (lexTable[i].LexemeType == LexemeTypes.Comment)
                    lexTable.RemoveAt(i--);
            }
        }

        private void Next()
        {
            index++;
        }

        private AstNode Analyze()
        {
            AstNode root = new AstNode(AstNodeTypes.Root, AstNodeTypes.Root.ToString());
            index = 0;

            while (index < lexTable.Count)
            {
                root.AddChild(Expr());
                Next();
            }

            return root;
        }

        private AstNode Expr()
        {
            int i = index;
            try
            {
                return Assignment();
            }
            catch (Exception ignore)
            {
                //ignored
            }

            index = i;
            try
            {
                return Condition();
            }
            catch (Exception ignore)
            {
                //ignored
            }

            index = i;
            throw new Exception($"expected assignment or condition expression: {Current.Name}");
        }

        private AstNode Id()
        {
            if (Current.LexemeType == LexemeTypes.Id)
            {
                return new AstNode(AstNodeTypes.Id, Current.Name);
            }

            throw new Exception($"expected id: {Current.Name}");
        }

        private AstNode Separator()
        {
            if (Current.LexemeType == LexemeTypes.Separator)
            {
                return new AstNode(AstNodeTypes.Separator, Current.Name);
            }

            throw new Exception($"expected separator: {Current.Name}");
        }

        private AstNode IdOrRomanNum()
        {
            switch (Current.LexemeType)
            {
                case LexemeTypes.Id:
                    return new AstNode(AstNodeTypes.Id, Current.Name);
                case LexemeTypes.RomanNum:
                    return new AstNode(AstNodeTypes.RomanNum, Current.Name);
                default:
                    throw new Exception($"expected id or roman number: {Current.Name}");
            }
        }

        private AstNode Assignment()
        {
            AstNode left = Id();

            Next();
            if (Current.LexemeType == LexemeTypes.Assignment)
            {
                AstNode ex = new AstNode(AstNodeTypes.Assignment, Current.Name);
                ex.AddChild(left);

                Next();
                AstNode right = IdOrRomanNum();
                ex.AddChild(right);

                Next();
                AstNode s = Separator();
                ex.AddChild(s);

                return ex;
            }

            throw new Exception($"expected assignment: {Current.Name}");
        }

        private AstNode Comparison()
        {
            AstNode left = IdOrRomanNum();

            Next();
            if (Current.LexemeType == LexemeTypes.Comparison)
            {
                AstNode ex = new AstNode(AstNodeTypes.Comparison, Current.Name);
                ex.AddChild(left);

                Next();
                AstNode right = IdOrRomanNum();
                ex.AddChild(right);

                return ex;
            }

            throw new Exception($"expected comparison: {Current.Name}");
        }

        private AstNode Condition()
        {
            if (Current.LexemeType == LexemeTypes.Condition && Current.Name == "if")
            {
                AstNode @if = new AstNode(AstNodeTypes.Condition, Current.Name);

                Next();
                AstNode c = Comparison();
                @if.AddChild(c);

                Next();
                if (Current.LexemeType == LexemeTypes.Condition && Current.Name == "then")
                {
                    AstNode @then = new AstNode(AstNodeTypes.Condition, Current.Name);
                    @if.AddChild(@then);

                    Next();
                    AstNode eThen = Assignment();
                    @then.AddChild(eThen);

                    Next();
                    if (Current.LexemeType == LexemeTypes.Condition && Current.Name == "else")
                    {
                        AstNode @else = new AstNode(AstNodeTypes.Condition, Current.Name);
                        @if.AddChild(@else);

                        Next();
                        AstNode eElse = Assignment();
                        @else.AddChild(eElse);
                    }

                    return @if;
                }

                throw new Exception($"expected condition word \'then\': {Current.Name}");
            }

            throw new Exception($"expected condition word \'if\': {Current.Name}");
        }
    }
}