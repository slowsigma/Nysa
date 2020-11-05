using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

namespace Nysa.Parse.Collections
{

    public class SparseNarrow<T>
    {
        public SparseNarrow<T> Create(T @default, (Int32 Index, T Value)[] items)
        {
            if (items.Length == 0)
                return new SparseNarrow<T>(@default, 0, new T[0]);

            items = items.OrderBy(i => i.Index).ToArray();

            var min = items[0].Index;
            var max = items[items.Length - 1].Index;

            var narrow = new T[(max - min) + 1];
            var j      = 0;

            for (Int32 i = 0; i < items.Length; i++)
            {
                while (j < items[i].Index)
                {
                    narrow[j] = @default;
                    j++;
                }

                narrow[j] = items[i].Value;
            }

            return new SparseNarrow<T>(@default, min, narrow);
        }

        private T     _Default;
        private Int32 _Start;
        private T[]   _Items;

        private SparseNarrow(T @default, Int32 start, T[] items)
        {
            this._Default = @default;
            this._Start   = start;
            this._Items   = items;
        }

        public T this[Int32 index]
            =>   index < this._Start ? this._Default
               : index - this._Start < this._Items.Length ? this._Items[index - this._Start]
               : this._Default;
    }

}
