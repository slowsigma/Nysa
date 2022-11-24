using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using SyntaxToken = Nysa.Text.Lexing.Token;
using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract class CodeNode
    {
        public Option<SyntaxToken> Token { get; private set; }
        public Option<SyntaxNode>  Node  { get; private set; }

        protected CodeNode(SyntaxToken token)
        {
            this.Token = token.Some();
            this.Node  = Option.None;
        }

        protected CodeNode(SyntaxNode node)
        {
            this.Token = Option.None;
            this.Node  = node.Some();
        }
    }

}
