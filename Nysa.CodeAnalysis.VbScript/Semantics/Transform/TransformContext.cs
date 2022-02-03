using System;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class TransformContext
    {
        public SyntaxNode               Node        { get; private set; }
        public Option<TransformContext> Previous    { get; private set; }

        private TransformContext(SyntaxNode node, Option<TransformContext> previous)
        {
            this.Node       = node;
            this.Previous   = previous;
        }
    
        public TransformContext(SyntaxNode root) : this(root, Option.None) { }

        public TransformContext ForMember(SyntaxNode member) => new TransformContext(member, this.Some());
    }

}