using System;

using Nysa.Logics;

namespace Nysa.Collections
{

    public class CharTrie<T> : Trie<Char, T>
        where T : notnull
    {
        public CharTrie(Option<CharTrie<T>> parent, Option<T> value, Trie.ChildrenFunction<Char, T> getChildren)
            : base(parent.Match(s => s.AsOption<Trie<Char, T>>(), () => Option<Trie<Char, T>>.None), value, getChildren)
        {
        }
    }

}
