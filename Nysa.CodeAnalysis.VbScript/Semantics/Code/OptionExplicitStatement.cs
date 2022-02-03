using Dorata.Logics;
using System;
using System.Collections.Generic;

using SyntaxNode  = Dorata.Text.Parsing.Node;

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
