using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text
{

    public struct TextSpan : IEquatable<TextSpan>
    {
        // static members
        public static Boolean operator ==(TextSpan lhs, TextSpan rhs) => lhs.Equals(rhs);
        public static Boolean operator !=(TextSpan lhs, TextSpan rhs) => !lhs.Equals(rhs);

        public static implicit operator TextSpan(TextPosition position) => new TextSpan(position.Source, position.Value, 0);

        // instance members
        public String  Source   { get; private set; }
        public Int32   Position { get; private set; }
        public Int32   Length   { get; private set; }

        public TextPosition Start => new TextPosition(this.Source, this.Position);
        public TextPosition End   => new TextPosition(this.Source, this.Position + this.Length);

        public TextSpan(String? source, Int32 position, Int32 length)
        {
            this.Source     = source ?? String.Empty;
            this.Position   = (position <= this.Source.Length) ? position : this.Source.Length;
            this.Length     = ((this.Position + length) <= this.Source.Length) ? length : this.Source.Length - this.Position;
        }

        public Boolean Equals(TextSpan other)
            => this.Source == other.Source && this.Position == other.Position && this.Length == other.Length;
        public override Boolean Equals(Object? obj)
            => obj is TextSpan span ? this.Equals(span) : false;
        public override Int32 GetHashCode()
            => this.Source.HashWithOther(this.Position, this.Length);
        public override String ToString()
            => this.Length > 0 ? Source.Substring(this.Position, this.Length) : String.Empty;
    }

}
