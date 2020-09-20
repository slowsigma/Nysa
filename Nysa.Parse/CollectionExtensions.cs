using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public static class CollectionExtensions
    {
        public static V Lookup<K, V>(this IDictionary<K, V> @this, K key, V notFoundValue)
            => @this.ContainsKey(key) ? @this[key] : notFoundValue;

        public static Option<T> FirstSome<T>(this IEnumerable<Option<T>> @this)
        {
            foreach (var item in @this)
            {
                if (item is Some<T> some)
                    return some;
            }

            return Option<T>.None;
        }

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> @this, Func<T, Boolean> predicate)
        {
            foreach (var item in @this)
            {
                if (predicate(item))
                    return item.Some();
            }

            return Option.None;
        }

    }

}
