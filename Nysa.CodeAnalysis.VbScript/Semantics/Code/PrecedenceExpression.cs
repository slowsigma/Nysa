using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxToken = Dorata.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (third option only)
     * <Value> ::=   <ConstExpr>
     *             | <LeftExpr>
     *             | "(" <Expr> ")"
     *             | "(" <Expr> ")." <LeftExprTail>
     */

    public class PrecedenceExpression : Expression
    {
        public Expression Value { get; private set; }

        public PrecedenceExpression(SyntaxToken source, Expression value)
            : base(source)
        {
            this.Value = value;
        }
    }
}
