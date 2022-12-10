using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Collections
{

    public static class Trie
    {
        public delegate IEnumerable<(TKey key, Trie<TKey, TValue> child)> ChildrenFunction<TKey, TValue>(Trie<TKey, TValue> parent)
            where TKey : IEquatable<TKey>
            where TValue : notnull;

        private static ChildrenFunction<TKey, TValue> CreateChildren<TKey, TValue>(Int32 keyPosition, IEnumerable<(TKey[] key, TValue value)> subset)
            where TKey : IEquatable<TKey>
            where TValue : notnull
            => p =>
            {
                var nextKeyPosition = keyPosition + 1;
                var groups          = subset.GroupBy(sk => sk.key[keyPosition]);

                return groups.Select(g => (key: g.Key, child: new Trie<TKey, TValue>(p.AsOption(),
                                                                                     g.Where(t => !(nextKeyPosition < t.key.Length)).Select(c => c.value).FirstOrNone(),
                                                                                     CreateChildren(nextKeyPosition, g.Where(i => nextKeyPosition < i.key.Length)))));
            };

        public static Trie<TKey, TValue> Create<TKey, TValue>(params (TKey[] key, TValue value)[] items)
            where TKey : IEquatable<TKey>
            where TValue : notnull
        {
            var value = items.Where(s => s.key.Length == 0).Select(t => t.value).FirstOrNone();

            return new Trie<TKey, TValue>(Option.None, value, CreateChildren(0, items.Where(i => i.key.Length > 0))); // always ensure subset key lengths are sufficient
        }

        public static Trie<TKey, TValue> Create<TKey, TValue>(IEnumerable<(TKey[] key, TValue value)> items)
            where TKey : IEquatable<TKey>
            where TValue : notnull
        {
            var value = items.Where(s => s.key.Length == 0).Select(t => t.value).FirstOrNone();

            return new Trie<TKey, TValue>(Option.None, value, CreateChildren(0, items.Where(i => i.key.Length > 0))); // always ensure subset key lengths are sufficient
        }

        public static Option<TValue> Find<TKey, TValue>(this Trie<TKey, TValue> @this, TKey[] key, Int32 keyPosition = 0)
            where TKey : IEquatable<TKey>
            where TValue : notnull
            => keyPosition < key.Length
               ? @this[key[keyPosition]].Bind(s => s.Find(key, keyPosition + 1))
               : @this.Value;

        public static CharTrie<T> Create<T>(params (String key, T value)[] items)
            where T : notnull
        {
            var value = items.Where(s => s.key.Length == 0)
                             .Select(t => t.value)
                             .FirstOrNone();

            return new CharTrie<T>(Option.None, value, CreateChildren(0, items.Select(i => (key: i.key.ToArray(), value: i.value))));
        }

        public static Option<T> Find<T>(this CharTrie<T> @this, String key, Int32 keyPosition = 0)
            where T : notnull
            => @this.Find(key.ToArray(), keyPosition);

    }

}
