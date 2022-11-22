using System;

using Nysa.Logics;

using Nysa.Text.Lexing;
using ParseId = Nysa.Text.Identifier;
using SyntaxToken = Nysa.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Maybe
    {

        // assumes current token might be one of a specific type (parseIds), only moves forward if so (no errors)
        public static Get<Option<SyntaxToken>> TokenOf(params ParseId[] parseIds)
            => (b, i) => i.Value < b.Length && b[i] is TokenItem item && parseIds.Any(p => item.Value.Id.IsEqual(p))
                         ? (item.Value.Some(), i.Value + 1)
                         : (Option<SyntaxToken>.None, i);

        // assumes current token might be one of a specific type <T>, only moves forward if so (no errors)
        public static Get<Option<T>> Node<T>()
            where T : CodeNode
            => (b, i) => i.Value < b.Length && b[i] is SemanticItem item && item.Value is T result
                         ? (result.Some(), i.Value + 1)
                         : (Option<T>.None, i);

    }

}