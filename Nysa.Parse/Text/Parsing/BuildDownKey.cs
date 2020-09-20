using System;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    // Key used to memoize building down.
    internal struct BuildDownKey : IEquatable<BuildDownKey>
    {
        // static members
        public static Boolean operator==(BuildDownKey lhs, BuildDownKey rhs) => lhs.Equals(rhs);
        public static Boolean operator!=(BuildDownKey lhs, BuildDownKey rhs) => !lhs.Equals(rhs);

        // instance members
        public ChartEntry     Entry       { get; private set; }
        public ChartPosition  Position    { get; private set; }

        public BuildDownKey(ChartEntry entry, ChartPosition position)
        {
            this.Entry      = entry;
            this.Position   = position;
        }

        public Boolean Equals(BuildDownKey other)
            =>    this.Entry.Equals(other.Entry)
               && this.Position.Equals(other.Position);

        public override Boolean Equals(Object other)
            =>    other is BuildDownKey
               && this.Equals((BuildDownKey)other);
        public override Int32 GetHashCode()
            => this.Entry.HashWithOther(this.Position);
    }

}
