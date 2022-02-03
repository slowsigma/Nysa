using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxToken = Dorata.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Get
    {
        public static Get<IEnumerable<SyntaxToken>> Split(this Get<SyntaxToken> get, Func<SyntaxToken, IEnumerable<SyntaxToken>> how)
            => (i, r) => get(i, r).Make(t => (Item: how(t.Item), Remainder: t.Remainder));

        public static Get<IEnumerable<TResult>> Cast<TItem, TResult>(this Get<IEnumerable<TItem>> get, Func<TItem, TResult> transform)
            => (i, r) => get(i, r).Make(t => (t.Item.Select(v => transform(v)), t.Remainder));

    }

}