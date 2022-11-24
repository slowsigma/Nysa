using System;
using System.Linq;

using Nysa.Logics;

using Nysa.Text.Lexing;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class CodeNodeExtensions
    {

        public static (Token? Start, Token? End, Token? StartPlusOne, Token? EndMinusOne) TokenBounds(this CodeNode @this)
        {
            var tokens = @this.Node.Match(n => n.OrderedTokens().ToArray(), () => new Token[] {});
            var length = tokens.Length;

            return length > 0
                   ? (Start: tokens[0], End: tokens[length - 1], StartPlusOne: length > 2 ? tokens[1] : null, EndMinusOne: length > 2 ? tokens[length - 2] : null)
                   : (Start: null, End: null, StartPlusOne: null, EndMinusOne: null);
        }

        public static Token LeadToken(this CodeNode @this)
            => @this.Node.Bind(n => n.OrderedTokens().FirstOrNone()) is Some<Token> someToken
               ? someToken.Value
               : throw new Exception("Unexpected type in call to LeadToken function.");

        // public static Token? FirstToken(this CodeNode @this, Nysa.Text.Identifier tokenId)
        // {
        //     return @this.Node.Match(n => n.Where(nt => nt.IsToken).Select(v => v.AsToken).FirstOrNone(tk => tk.Id.Equals(tokenId)).Match(f => f, () => (Token?)null),
        //                             () => (Token?)null);
        // }

    }

}