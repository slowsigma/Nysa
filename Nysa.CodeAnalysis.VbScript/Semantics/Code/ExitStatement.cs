using System;
using System.Collections.Generic;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <ExitStmt> ::= "Exit" "Do" | "Exit" "For" | "Exit" "Function" | "Exit" "Property" | "Exit" "Sub"
     */

    public class ExitStatement : Statement
    {
        public ExitTypes Type { get; private set; }

        public ExitStatement(SyntaxNode source, ExitTypes exitType) : base(source) { this.Type = exitType; }
    }

}
