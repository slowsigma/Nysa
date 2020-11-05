using System;
using System.Collections.Generic;

namespace Nysa.Logics
{

    public static class Swaps
    {

        public static (T second, T first) Swapped<T>(this T first, T second)
            => (second, first);

        public static void Swap<T>(this IList<T> @this, Int32 first, Int32 second)
        {
            var t         = @this[first];
            @this[first]  = @this[second];
            @this[second] = t;
        }

    }

}
