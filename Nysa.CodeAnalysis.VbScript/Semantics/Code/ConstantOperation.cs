using System;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (second and third options)
     * <ConstExprDef> ::=   "(" <ConstExprDef> ")"
     *                    | "-" <ConstExprDef>
     *                    | "+" <ConstExprDef>
     *                    | <ConstExpr>
     */

    public class ConstantOperation : ConstantExpression
    {
        public ConstantOperationTypes   Type    { get; private set; }
        public ConstantExpression       Operand { get; private set; }   // ConstantOperation | LiteralValue

        public ConstantOperation(SyntaxNode source, ConstantOperationTypes type, ConstantExpression operand)
            : base(source)
        {
            this.Type    = type;
            this.Operand = operand;
        }
    }

}
