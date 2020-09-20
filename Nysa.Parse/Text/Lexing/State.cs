using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing
{

    public struct State
    {
        public static State Hit(TextSpan span, Identifier? identifier = null)
            => new State(span, true, identifier ?? Identifier.None);
        public static State Miss(TextSpan span, Identifier? identifier = null)
            => new State(span, false, identifier ?? Identifier.None);

        // instance members
        public TextSpan Span { get; private set; }
        public Boolean IsHit { get; private set; }
        public Boolean IsMiss => !this.IsHit;
        public Identifier Identifier { get; private set; }
        public Int32 Size => this.Span.Length;

        public State(TextSpan span, Boolean isHit, Identifier identifier)
        {
            this.Span       = span;
            this.IsHit      = isHit;
            this.Identifier = identifier;
        }

        public override String ToString()
            => this.IsHit
               ? $"{this.Identifier} '{this.Span}'"
               : $"({this.Identifier}) {this.Span.Position}[{this.Span.Length}]";
    }

}
