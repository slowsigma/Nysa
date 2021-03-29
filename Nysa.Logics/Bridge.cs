using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nysa.Logics
{

    public static class Bridge
    {
        [Obsolete("Misleading name, use Affect instead.")]
        public static Unit Send<T>(this T @this, Action<T> action)
        {
            action(@this);
            return Unit.Value;
        }

        [Obsolete("Misleading name, use Affect instead.")]
        public static Unit Send<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
                action(item);

            return Unit.Value;
        }

        /// <summary>
        /// Takes a value of some type, calls an update action
        /// on that value, and then returns the value. Because
        /// the value is returned by this function, it's implied
        /// that the update action alters the supplied value.
        /// </summary>
        public static T Affected<T>(this T @this, Action<T> update)
        {
            update(@this);

            return @this;
        }

        /// <summary>
        /// Takes a series of some type, calls an update action
        /// on each value, and returns each value.  It's implied
        /// that the update action alters each value before it's
        /// returned.
        /// </summary>
        public static IEnumerable<T> Affected<T>(this IEnumerable<T> @this, Action<T> update)
        {
            foreach (var value in @this)
            {
                update(value);

                yield return value;
            }
        }

        /// <summary>
        /// Takes a value of some type and calls an update action
        /// with that value. The update action may affect the
        /// supplied value or may change some other state.
        /// </summary>
        public static Unit Affect<T>(this T @this, Action<T> update)
        {
            update(@this);

            return Unit.Value;
        }

        /// <summary>
        /// Takes a series of some type and calls an update action
        /// on each value. The update action may affect each value
        /// or may change some other state.
        /// </summary>
        public static Unit Affect<T>(this IEnumerable<T> @this, Action<T> update)
        {
            foreach (var value in @this)
            {
                update(value);
            }

            return Unit.Value;
        }

        public static Int32 HashWithSame<T>(this T start, params T[] others)
            => others.Aggregate((17 * 31) + start.GetHashCode(), (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.

        public static Int32 HashWithOther<T, U>(this T start, params U[] others)
            => others.Aggregate((17 * 31) + start.GetHashCode(), (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.

        public static Int32 HashAll<T>(this IEnumerable<T> items)
            => items.Aggregate(17, (s, i) => (s * 31) + i.GetHashCode()); // The Albahari method.

        public static Func<T, T, Int32> ToFunction<T>(this IComparer<T> @this)
            where T : IComparable<T>
            => (a, b) => @this.Compare(a, b);

        public static Boolean OrdinalEquals(this String @this, String other, Boolean ignoreCase = true)
            => String.Equals(@this, other, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

    }

}
