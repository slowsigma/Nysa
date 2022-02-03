using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class Identifier : CodeNode
    {
        public new SyntaxNode Node { get; private set; }
        public String Value { get; private set; }

        public Identifier(SyntaxNode source, String value)
            : base(source)
        {
            this.Node  = source;
            this.Value = value;
        }

        public override String ToString() => this.Value;
    }

}
