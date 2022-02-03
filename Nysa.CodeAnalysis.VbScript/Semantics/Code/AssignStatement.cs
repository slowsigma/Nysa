using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <AssignStmt> ::= <LeftExpr> "=" <Expr> | "Set" <LeftExpr> "=" <Expr>
     */

    public class AssignStatement : Statement
    {
        public Boolean          Set   { get; private set; }
        public AccessExpression Left  { get; private set; }
        public Expression       Right { get; private set; }

        public AssignStatement(SyntaxNode source, Boolean set, AccessExpression left, Expression right)
            : base(source)
        {
            this.Set    = set;
            this.Left   = left;
            this.Right  = right;
        }
    }

}
