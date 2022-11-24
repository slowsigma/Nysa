using System;
using System.Collections.Generic;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <OptionExplicit> ::= "Option" "Explicit" <NL>
     */

    public class OptionExplicitStatement : Statement
    {
        public OptionExplicitStatement(SyntaxNode source) : base(source) { }
    }

}
