using System;
using System.Collections.Generic;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <ConstExprDef> ::=   "(" <ConstExprDef> ")"
     *                    | "-" <ConstExprDef>
     *                    | "+" <ConstExprDef>
     *                    | <ConstExpr>
     */

    public abstract class ConstantExpression : Expression
    {
        protected ConstantExpression(SyntaxNode source) : base(source) { }
    }

}
