using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public partial class Node
    {

        // Key used to memoize building down.
        private struct DownKey : IEquatable<DownKey>
        {
            // static members
            public static Boolean operator==(DownKey lhs, DownKey rhs) => lhs.Equals(rhs);
            public static Boolean operator!=(DownKey lhs, DownKey rhs) => !lhs.Equals(rhs);

            // instance members
            public ChartEntry     Entry       { get; private set; }
            public ChartPosition  Position    { get; private set; }

            public DownKey(ChartEntry entry, ChartPosition position)
            {
                this.Entry      = entry;
                this.Position   = position;
            }

            public Boolean Equals(DownKey other)
                =>    this.Entry.Equals(other.Entry)
                   && this.Position.Equals(other.Position);

            public override Boolean Equals(Object? other)
                =>    other is DownKey down
                   && this.Equals(down);
            public override Int32 GetHashCode()
                => this.Entry.HashWithOther(this.Position);
        }

    }

}
