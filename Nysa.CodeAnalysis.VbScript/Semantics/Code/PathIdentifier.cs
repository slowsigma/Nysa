using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SyntaxToken = Dorata.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class PathIdentifier : PathExpressionItem
    {
        public String Value { get; private set; }

        public PathIdentifier(SyntaxToken source, String value)
            : base(source)
        {
            this.Value = value;
        }

        public override String ToString() => this.Value;
    }

}
