using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <CallStmt>    ::= "Call" <LeftExpr>
     */

    public class CallStatement : Statement
    {
        public AccessExpression AccessExpression { get; private set; }

        public CallStatement(SyntaxNode source, AccessExpression accessExpression)
            : base(source)
        {
            this.AccessExpression = accessExpression;
        }
    }

}
