using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <RedimDecl> ::= <ExtendedID> "(" <ExprList> ")"
     */

    public class RedimVariable : CodeNode
    {
        public Identifier       Name            { get; private set; }
        public ExpressionList   RankExpressions { get; private set; }

        public RedimVariable(SyntaxNode source, Identifier name, ExpressionList rankExpressions)
            : base(source)
        {
            this.Name            = name;
            this.RankExpressions = rankExpressions;
        }
    }

}
