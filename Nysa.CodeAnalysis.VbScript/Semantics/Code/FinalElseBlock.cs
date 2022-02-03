using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class FinalElseBlock : ElseBlock
    {
        public FinalElseBlock(SyntaxNode source, IEnumerable<Statement> statements) : base(source, statements) { }
    }

}
