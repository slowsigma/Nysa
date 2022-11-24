using System;
using System.Collections.Generic;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract class Statement : CodeNode
    {
        protected Statement(SyntaxNode source) : base(source) { }
    }

}
