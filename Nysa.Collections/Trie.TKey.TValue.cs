using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Collections
{

    public class Trie<TKey, TValue> : IEnumerable<(TKey Key, Trie<TKey, TValue> Value)>
        where TKey : IEquatable<TKey>
        where TValue : notnull
    {
        public Option<Trie<TKey, TValue>> Parent { get; private set; }
        public Option<TValue>             Value  { get; private set; }

        private Dictionary<TKey, Trie<TKey, TValue>> _Children;

        public Trie(Option<Trie<TKey, TValue>> parent, Option<TValue> value, Trie.ChildrenFunction<TKey, TValue> getChildren)
        {
            this.Parent     = parent;
            this.Value      = value;
            this._Children  = getChildren(this).ToDictionary(k => k.key, v => v.child);
        }

        public Option<Trie<TKey, TValue>> this[TKey index]
            => this._Children.ContainsKey(index) ? this._Children[index].AsOption() : Option.None;

        public IEnumerator<(TKey Key, Trie<TKey, TValue> Value)> GetEnumerator()
            => this._Children.Select(kv => (Key: kv.Key, Value: kv.Value)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }

}
