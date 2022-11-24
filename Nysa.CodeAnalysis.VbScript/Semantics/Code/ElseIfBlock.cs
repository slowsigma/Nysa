using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class ElseIfBlock : ElseBlock
    {
        public Expression Predicate  { get; private set; }

        public ElseIfBlock(SyntaxNode source, Expression predicate, IEnumerable<Statement> statements)
            : base(source, statements)
        {
            this.Predicate = predicate;
        }
    }

}
