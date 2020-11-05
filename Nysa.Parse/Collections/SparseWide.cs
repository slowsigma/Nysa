using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

namespace Nysa.Parse.Collections
{

    public class SparseWide<T>
    {
        private T                    _Default;
        private Dictionary<Int32, T> _Items;

        public SparseWide(T @default, (Int32 Index, T Value)[] items)
        {
            this._Default = @default;
            this._Items   = items.ToDictionary(k => k.Index, v => v.Value);
        }

        public T this[Int32 index]
            => this._Items.ContainsKey(index)
               ? this._Items[index]
               : this._Default;
    }

}
