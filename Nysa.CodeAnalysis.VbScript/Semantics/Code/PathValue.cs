using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxToken = Nysa.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (fourth option, see note)
     * <Value> ::=   <ConstExpr>
     *             | <LeftExpr>
     *             | "(" <Expr> ")"
     *             | "(" <Expr> ")." <LeftExprTail>     // <<-- Just the "(" <Expr> ")" before the dot.
     */

    public class PathValue : PathExpressionItem
    {
        public Expression Expression { get; private set; }

        public PathValue(SyntaxToken source, Expression expression)
            : base(source)
        {
            this.Expression = expression;
        }
    }

}
