using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SyntaxToken = Dorata.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract class PathExpressionItem : CodeNode
    {
        protected PathExpressionItem(SyntaxToken source) : base(source) { }
    }

}
