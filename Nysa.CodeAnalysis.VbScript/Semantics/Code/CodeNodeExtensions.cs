using System;
using System.Linq;

using Nysa.Logics;

using Dorata.Text.Lexing;
using Dorata.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class CodeNodeExtensions
    {

        public static (Token? Start, Token? End) TokenBounds(this CodeNode @this)
        {
            var tokens = @this.Node.Match(n => n.Where(nt => nt.IsToken).Select(v => v.AsToken).ToArray(), () => new Token[] {});

            return tokens.Length > 0
                   ? (Start: tokens[0], End: tokens[tokens.Length - 1])
                   : (Start: null, End: null);
        }

        public static Token? FirstToken(this CodeNode @this, Dorata.Text.Identifier tokenId)
        {
            return @this.Node.Match(n => n.Where(nt => nt.IsToken).Select(v => v.AsToken).FirstOrNone(tk => tk.Id.Equals(tokenId)).Match(f => f, () => (Token?)null),
                                    () => (Token?)null);
        }

    }

}