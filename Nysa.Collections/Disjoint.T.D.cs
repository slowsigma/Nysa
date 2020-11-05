using System;
using System.Collections.Generic;

using Nysa.Logics;

namespace Nysa.Collections
{

    public class Disjoint<T, D> where T : IEquatable<T>
    {
        private struct Entry
        {
            public T        Parent  { get; private set; }
            public D        Data    { get; private set; }
            public Int32    Rank    { get; private set; }
            public Entry(T parent, D data, Int32 rank)
            {
                this.Parent = parent;
                this.Data   = data;
                this.Rank   = rank;
            }
        }

        // instance members
        private Dictionary<T, Entry> _Entries;
        private Func<D, D, D>        _MergeData;

        public Disjoint(Func<D, D, D> mergeData)
        {
            this._Entries   = new Dictionary<T, Entry>();
            this._MergeData = mergeData;
        }

        public T MakeSet(T value, D data)
        {
            if (this.FindSet(value) is Some<T> some)
                return some.Value;

            this._Entries.Add(value, new Entry(value, data, 1));

            return value;
        }

        public Option<T> FindSet(T value)
        {
            if (!this._Entries.ContainsKey(value))
                return Option<T>.None;

            while (!this._Entries[value].Parent.Equals(value))
                value = this._Entries[value].Parent;

            return value.Some();
        }

        public Suspect<T> UnionSets(T a, T b)
        {
            var sa = this.FindSet(a);
            var sb = this.FindSet(b);

            if (sa is Some<T> someSa && sb is Some<T> someSb)
            {
                if (someSa.Value.Equals(someSb.Value))
                    return someSa.Value.Confirmed();

                var ea = this._Entries[someSa.Value];
                var eb = this._Entries[someSb.Value];
                var md = this._MergeData(ea.Data, eb.Data);

                if (ea.Rank < eb.Rank)
                {
                    this._Entries[someSa.Value] = new Entry(someSb.Value, ea.Data, ea.Rank);
                    this._Entries[someSb.Value] = new Entry(eb.Parent, md, eb.Rank);

                    return someSb.Value.Confirmed();
                }
                else
                {
                    this._Entries[someSb.Value] = new Entry(someSa.Value, eb.Data, eb.Rank);
                    this._Entries[someSa.Value] = new Entry(ea.Parent, md, ea.Rank == eb.Rank ? ea.Rank + 1 : ea.Rank);

                    return someSa.Value.Confirmed();
                }
            }
            else
            {
                return (new ArgumentException("Both a and b must be in a valid set to perform a union.")).Failed<T>();
            }
        }

    }

}
