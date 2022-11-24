using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SyntaxToken = Nysa.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class PathWith : PathExpressionItem
    {
        public PathWith(SyntaxToken source) : base(source) { }
    }

}
