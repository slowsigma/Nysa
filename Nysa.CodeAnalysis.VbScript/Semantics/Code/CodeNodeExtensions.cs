using System;
using System.Linq;

using Nysa.Logics;

using Dorata.Text.Lexing;
using Dorata.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class CodeNodeExtensions
    {

        public static Option<(Token Start, Token End)> TokenBounds(this CodeNode @this)
        {
            var tokens = @this.Node.Match(n => n.Where(nt => nt.IsToken).Select(v => v.AsToken).ToArray(), () => new Token[] {});

            return tokens.Length > 0
                   ? (Start: tokens[0], End: tokens[tokens.Length - 1]).Some()
                   : Option.None;
        }

    }

}