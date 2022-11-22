using System;
using System.Collections.Generic;

using SyntaxToken = Nysa.Text.Lexing.Token;
using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract class Expression : CodeNode
    {
        protected Expression(SyntaxNode source) : base(source) { }
        protected Expression(SyntaxToken source) : base(source) { }
    }

}
