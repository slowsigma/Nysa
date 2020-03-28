using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public sealed class None<T> : Option<T>
    {
        public static IEnumerable<T> Return() { yield break; }
        internal None() { }
        public override String ToString() => "{Option.None}";
    }

}
