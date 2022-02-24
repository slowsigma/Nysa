using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text
{

    public struct Identifier : IEquatable<Identifier>, IEquatable<Int32>
    {
        // static members
        public static Boolean operator==(Identifier lhs, Identifier rhs) => lhs.Equals(rhs);
        public static Boolean operator!=(Identifier lhs, Identifier rhs) => !lhs.Equals(rhs);

        public static Identifier FromInteger(Int32 value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException("value", "Identifier cannot be created from zero or a negative number.");

            return new Identifier(value);
        }

        /// <summary>
        /// Used to indicate no identifier or that a token is invalid.
        /// </summary>
        public static readonly Identifier None = new Identifier(0);
        /// <summary>
        /// Used to indicate a token that is trivial or serves no purpose in some process.
        /// </summary>
        public static readonly Identifier Trivia = new Identifier(Int32.MinValue);

        // instance members
        private Int32 _Value;

        private Identifier(Int32 value) { this._Value = value; }

        public Boolean IsNone { get => this._Value == 0; }

        public Boolean Equals(Identifier other)
            => this._Value == other._Value;
        public Boolean Equals(Int32 other)
            => this._Value == other;
        public override Boolean Equals(Object? other)
            =>    other is Identifier id   ? this.Equals(id)
                : other is Int32      @int ? this.Equals(@int)
                :                            false;
        public override Int32 GetHashCode()
            => this._Value.GetHashCode();
        public override String ToString()
            => this._Value.ToString();
    }

}
