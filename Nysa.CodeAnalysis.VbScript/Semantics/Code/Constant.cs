using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <ConstList> ::=   <ExtendedID> "=" <ConstExprDef> "," <ConstList>
     *                 | <ExtendedID> "=" <ConstExprDef>
     *                 | <ExtendedID> "," <ConstList>
     *                 | <ExtendedID>
     */

    public class Constant : CodeNode
    {
        public Identifier                   Name          { get; private set; }
        public Option<ConstantExpression>   Expression    { get; private set; }

        public Constant(SyntaxNode source, Identifier name, Option<ConstantExpression> expression)
            : base(source)
        {
            this.Name       = name;
            this.Expression = expression;
        }
    }

}
