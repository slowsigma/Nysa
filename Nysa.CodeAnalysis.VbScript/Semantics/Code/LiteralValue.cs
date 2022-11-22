using System;
using System.Linq;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    /*
     * (fourth option)
     * <ConstExprDef> ::=   "(" <ConstExprDef> ")"
     *                    | "-" <ConstExprDef>
     *                    | "+" <ConstExprDef>
     *                    | <ConstExpr>
     * 
     * <ConstExpr> ::=   <BoolLiteral>
     *                 | <IntLiteral>
     *                 | {FloatLiteral}
     *                 | {StringLiteral}
     *                 | {DateLiteral}
     *                 | <Nothing>
     * 
     * <Nothing> ::= "Nothing" | "Null" | "Empty"
     *
     * <IntLiteral> ::=   {IntLiteral}
     *                  | {HexLiteral}
     *                  | {OctLiteral}
     * 
     * <BoolLiteral> ::= "True" | "False"
     */

    public enum LiteralValueTypes : Int32
    {
        Boolean,
        Integer,
        Float,
        String,
        Date,
        Nothing,
        Null,
        Empty
    }

    public class LiteralValue : ConstantExpression
    {
        public LiteralValueTypes Type { get; private set; }
        public String Value { get; private set; }

        public LiteralValue(SyntaxNode source, LiteralValueTypes type, String value)
            : base(source)
        {
            this.Type  = type;
            this.Value = value;
        }

        public override String ToString() => this.Value;
    }

}
