using System;

using Nysa.Logics;

using SyntaxToken = Dorata.Text.Lexing.Token;
using SytnaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class Either
    {
        public Transform Make<TResult>(Func<CodeNode, TResult> howWhenNode, Func<SytnaxNode, SyntaxToken, TResult> howWhenToken)
            where TResult : CodeNode
            => (c, m) => new TransformItem[] { (SemanticItem)m[Index.Start].Match(howWhenNode, st => howWhenToken(c.Node, st)) };

        public Transform Make<TResult>(Func<CodeNode, TResult> howWhenNode, Func<SytnaxNode, SyntaxToken, TransformItem, TResult> howWhenTokenAndNext)
            where TResult : CodeNode
            => (c, m) => new TransformItem[] { (SemanticItem)m[Index.Start].Match(howWhenNode, st => howWhenTokenAndNext(c.Node, st, m[Index.Start.Value + 1])) };
    }

}