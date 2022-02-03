using System;

using Nysa.Logics;

using SyntaxToken = Dorata.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class TransformItemFunctions
    {
        public static T Match<T>(this TransformItem @this, Func<CodeNode, T> whenNode, Func<SyntaxToken, T> whenToken)
            =>   @this is SemanticItem  node  ? whenNode(node.Value)
               : @this is TokenItem     token ? whenToken(token.Value)
               :                                throw new ArgumentOutOfRangeException($"Match only accepts {nameof(SemanticItem)} and {nameof(TokenItem)} types.");
    }

}