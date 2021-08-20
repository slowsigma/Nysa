using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public sealed class None<T> : Option<T>
    {
        [Obsolete("Return on None<T> is deprecated, please use Enumerable instead.")]
        public static IEnumerable<T> Return() { yield break; }

        public static IEnumerable<T> Enumerable() { yield break; }
        
        internal None() { }
        public override String ToString() => "{Option.None}";
    }

}
