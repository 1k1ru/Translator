using System;
using System.Collections.Generic;
using System.Text;

namespace Translator.SyntacticAnalyzer
{
    public enum AstNodeTypes
    {
        Root,
        Id,
        RomanNum,
        Condition,
        Separator,
        Comparison,
        Assignment
    }
}
