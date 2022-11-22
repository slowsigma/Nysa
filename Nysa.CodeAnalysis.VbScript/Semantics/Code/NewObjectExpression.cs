using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <NewObjectExpr> ::= "New" <LeftExpr>
     */

    public class NewObjectExpression : AccessExpression
    {
        public AccessExpression Object { get; private set; }

        public NewObjectExpression(SyntaxNode source, AccessExpression @object)
            : base(source)
        {
            this.Object = @object;
        }
    }

}
