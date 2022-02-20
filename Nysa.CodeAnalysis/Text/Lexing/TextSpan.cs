using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;

namespace Dorata.Text
{

    public struct TextSpan : IEquatable<TextSpan>
    {
        // static members
        public static Boolean operator ==(TextSpan lhs, TextSpan rhs) => lhs.Equals(rhs);
        public static Boolean operator !=(TextSpan lhs, TextSpan rhs) => !lhs.Equals(rhs);

        public static implicit operator TextSpan(TextPosition position) => new TextSpan(position.Source, position.Value, 0);
        public static implicit operator TextSpan(String source) => new TextSpan(source ?? String.Empty, 0, (source ?? String.Empty).Length);

        // instance members
        public String  Source   { get; private set; }
        public Int32   Position { get; private set; }
        public Int32   Length   { get; private set; }
        public String  Value    { get => this.Length > 0 ? Source.Substring(this.Position, this.Length) : String.Empty; }
        public Boolean IsEmpty  { get => this.Length == 0; }

        public TextPosition Start { get => new TextPosition(this.Source, this.Position); }
        public TextPosition End   { get => new TextPosition(this.Source, this.Position + this.Length); }

        public TextSpan(String? source, Int32 position, Int32 length)
        {
            this.Source     = source ?? String.Empty;
            this.Position   = (position <= this.Source.Length) ? position : this.Source.Length;
            this.Length     = ((this.Position + length) <= this.Source.Length) ? length : this.Source.Length - this.Position;
        }

        public TextSpan Appended(Int32 length)
            => new TextSpan(this.Source, this.Position, this.Length + length);

        public TextSpan Appended(Func<Char, Boolean> whileTrue)
        {
            var check = this.Position + this.Length;

            while (check < this.Source.Length && whileTrue(this.Source[check]))
                check++;

            return new TextSpan(this.Source, this.Position, check - 1);
        }

        public TextSpan? TryExpand(Char nextChar)
            => !this.End.IsEnd && (this.Source[this.End.Value] == nextChar) ? this.Appended(this.Length + 1) : (TextSpan?)null;

        public TextSpan Plus(TextSpan other)
            => new TextSpan(this.Source, this.Position, (other.Position + other.Length) - this.Position);
        public Boolean Equals(TextSpan other)
            => this.Source == other.Source && this.Position == other.Position && this.Length == other.Length;
        public override Boolean Equals(Object? obj)
            => obj is TextSpan span ? this.Equals(span) : false;
        public override Int32 GetHashCode()
            => this.Source.HashWithOther(this.Position, this.Length);

    }

}
