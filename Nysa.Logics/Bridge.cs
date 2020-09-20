using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nysa.Logics
{

    public static class Bridge
    {
        public static Unit Send<T>(this T @this, Action<T> action)
        {
            action(@this);
            return Unit.Value;
        }

        public static Unit Send<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
                action(item);

            return Unit.Value;
        }

        public static Int32 HashWithSame<T>(this T start, params T[] others)
            => others.Aggregate((17 * 31) + start.GetHashCode(), (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.

        public static Int32 HashWithOther<T, U>(this T start, params U[] others)
            => others.Aggregate((17 * 31) + start.GetHashCode(), (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.

        public static Int32 HashAll<T>(this IEnumerable<T> items)
            => items.Aggregate(17, (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.
    }

}
