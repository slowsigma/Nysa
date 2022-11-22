using System;

using SyntaxToken = Nysa.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed class TokenItem : TransformItem
    {
        public static implicit operator TokenItem(SyntaxToken syntaxToken) => new TokenItem(syntaxToken);

        public SyntaxToken Value { get; private set; }

        public TokenItem(SyntaxToken value) { this.Value = value; }
    }

}