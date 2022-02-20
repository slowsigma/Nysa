using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Logics
{

    public static class LogicsExtensions
    {

        // public static Int32 HashWithSame<T>(this T start, params T[] others)
        //     => others.Aggregate((17 * 31) + start.GetHashCode(), (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.

        public static Int32 HashWithOther<T, U>(this T start, params U[] others)
            where T : notnull
            where U : notnull
            => others.Aggregate((17 * 31) + start.GetHashCode(), (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.

        public static Int32 HashAcross<T>(this IEnumerable<T> items)
            where T : notnull
            => items.Aggregate(17, (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.
            
    }

}
