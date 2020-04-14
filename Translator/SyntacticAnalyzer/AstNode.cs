using System;
using System.Collections.Generic;
using Translator.LexicalAnalyzer;

namespace Translator.SyntacticAnalyzer
{
    public class AstNode
    {
        private List<AstNode> Children { get; }

        public AstNodeTypes Type { get; set; }
        public string Name { get; set; }

        public AstNode(AstNodeTypes type)
        {
            this.Type = type;

            Children = new List<AstNode>();
        }

        public AstNode(AstNodeTypes type, string name) : this(type)
        {
            this.Name = name;
        }

        public void AddChild(AstNode node)
        {
            this.Children.Add(node);
        }

        public void RemoveChild(int index)
        {
            this.Children.RemoveAt(index);
        }

        public void Print(string prefix = "")
        {
            Console.WriteLine(Name);

            for (int i = 0; i < Children.Count; i++)
            {
                Console.Write(prefix);
                Console.Write(i == Children.Count - 1 ? '└' : '├');

                string nextPrefix = prefix;
                if (i == Children.Count - 1)
                    nextPrefix += " ";
                else
                    nextPrefix += "│";

                Children[i].Print(nextPrefix);
            }
        }

        public AstNode this[int index] => Children[index];
    }
}