using System;

using Nysa.Text.Lexing;
using ParseId = Nysa.Text.Identifier;
using SyntaxToken = Nysa.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Expect
    {

        // Note: If no token throw error.
        public static Get<SyntaxToken> Token()
            => (b, i) => b[i] is TokenItem token
                         ? (token.Value, i.Value + 1)
                         : throw new Exception(With.MISSING_TOKEN);

        // Note: If no token throw error.
        public static Get<String> TokenValue()
            => (b, i) => b[i] is TokenItem token
                         ? (token.Value.Span.ToString(), i.Value + 1)
                         : throw new Exception(With.MISSING_TOKEN);

        // Note: If no token found of one of the given types throw error.
        public static Get<SyntaxToken> TokenOf(params ParseId[] parseIds)
            => (b, i) => b[i] is TokenItem token && parseIds.Any(p => token.Value.Id.IsEqual(p))
                         ? (token.Value, i.Value + 1)
                         : throw new Exception(With.MISSING_TOKEN);

        // Note: If no node T found at the current position throw error.
        public static Get<T> Node<T>()
            where T : CodeNode
            => (b, i) => b[i] is SemanticItem node && node.Value is T codeNode
                         ? (codeNode, i.Value + 1)
                         : throw new Exception(With.MISSING_NODE);

        // Note: If not all the remaining items are tokens throw error.
        public static Get<IEnumerable<SyntaxToken>> Tokens()
            => (b, i) =>
            {
                var items = new List<SyntaxToken>();

                for (Int32 j = i.Value; j < b.Length; j++)
                {
                    if (b[j] is TokenItem tokenItem)
                        items.Add(tokenItem.Value);
                    else
                        throw new Exception(With.UNEXPECTED_NODE);
                }

                return (items, Index.End);
            };

    }

}