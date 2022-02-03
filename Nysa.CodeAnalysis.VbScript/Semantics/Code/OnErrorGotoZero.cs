using System;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (second option)
     * <ErrorStmt> ::=   "On" "Error" "Resume" "Next"
     *                 | "On" "Error" "GoTo" {IntLiteral}
     */

    public class OnErrorGotoZero : Statement
    {
        public OnErrorGotoZero(SyntaxNode source) : base(source) { }
    }

}
