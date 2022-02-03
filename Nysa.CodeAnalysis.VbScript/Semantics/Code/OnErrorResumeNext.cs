using System;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (first option)
     * <ErrorStmt> ::=   "On" "Error" "Resume" "Next"
     *                 | "On" "Error" "GoTo" {IntLiteral}
     */

    public class OnErrorResumeNext : Statement
    {
        public OnErrorResumeNext(SyntaxNode source) : base(source) { }
    }

}
