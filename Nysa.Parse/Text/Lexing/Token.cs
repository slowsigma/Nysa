﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.Text.Lexing
{

    public struct Token : IEquatable<Token>
    {
        public static Boolean operator ==(Token lhs, Token rhs) => lhs.Equals(rhs);
        public static Boolean operator !=(Token lhs, Token rhs) => !lhs.Equals(rhs);

        // instance members
        public TextSpan   Span { get; private set; }
        public Identifier Id   { get; private set; }

        public Token(TextSpan span, Identifier id)
        {
            this.Span = span;
            this.Id   = id;
        }

        public Boolean Equals(Token other) => this.Span == other.Span && this.Id == other.Id;
        public override Boolean Equals(object obj) => (obj is Token) ? this.Equals((Token)obj) : false;
        public override Int32 GetHashCode() => this.Span.HashWithOther(this.Id);
        public override String ToString() => this.Span.Value;
    }

}
