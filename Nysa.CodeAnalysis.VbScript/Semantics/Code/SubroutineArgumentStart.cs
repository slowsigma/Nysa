using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxToken = Dorata.Text.Lexing.Token;
using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    // This is just a temporary container (see ParseIdTransforms.cs for details).
    public class SubroutineArgumentStart : CodeNode
    {
        public Option<Expression> Argument { get; private set; }

        public SubroutineArgumentStart(SyntaxToken source, Option<Expression> argument)
            : base(source)
        {
            this.Argument = argument;
        }

        public SubroutineArgumentStart(SyntaxNode source, Option<Expression> argument)
            : base(source)
        {
            this.Argument = argument;
        }
    }

}
