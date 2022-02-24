using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;

namespace Nysa.Text.Parsing
{

    public partial class Chart
    {

        public struct Position : IEquatable<Position>, IEnumerable<Entry>
        {
            // static members
            public static Boolean operator ==(Position lhs, Position rhs) => lhs.Equals(rhs);
            public static Boolean operator !=(Position lhs, Position rhs) => !lhs.Equals(rhs);

            // instance members
            public Chart  Chart { get; private set; }
            public Int32  Index { get; private set; }
            public Position(Chart chart, Int32 index)
            {
                this.Chart = chart;
                this.Index = index;
            }

            public Int32 Count { get => this.Chart._Data[this.Index] == null ? 0 : this.Chart._Data[this.Index].Count; }
            public Entry this[Int32 entry] { get => this.Chart._Data[this.Index][entry]; }

            public IEnumerable<Entry> Where(Func<Entry, Boolean> predicate)
                => (this.Chart._Data[this.Index] != null)
                   ? this.Chart._Data[this.Index].Where(predicate)
                   : Enums.None<Entry>();

            public IEnumerable<Entry> GetEntries()
            {
                if (this.Chart._Data[this.Index] == null)
                    yield break;

                for (Int32 e = 0; e < this.Chart._Data[this.Index].Count; e++)
                    yield return this.Chart._Data[this.Index][e];
            }

            public IEnumerator<Entry> GetEnumerator()
            {
                if (this.Chart._Data[this.Index] == null)
                    yield break;

                for (Int32 e = 0; e < this.Chart._Data[this.Index].Count; e++)
                    yield return this.Chart._Data[this.Index][e];
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            internal Int32 AddUnique(Chart.Entry entry)
            {
                if (this.Chart._Data[this.Index] == null)
                    this.Chart._Data[this.Index] = new List<Entry>();

                var entries     = this.Chart._Data[this.Index];
                var entryIndex  = entries.IndexOf(entry);

                if (entryIndex < 0)
                {
                    entryIndex = entries.Count;
                    entries.Add(entry);
                }

                return entryIndex;
            }

            internal Int32 AddUnique(Grammar.Rule rule, Int32 origin, Int32 next)
            {
                if (this.Chart._Data[this.Index] == null)
                    this.Chart._Data[this.Index] = new List<Entry>();

                var entries     = this.Chart._Data[this.Index];
                var entry       = new Entry(rule, origin, next);
                var entryIndex  = entries.IndexOf(entry);

                if (entryIndex < 0)
                {
                    entryIndex = entries.Count;
                    entries.Add(entry);
                }

                return entryIndex;
            }

            internal Int32 AddRaw(Entry entry)
            {
                if (this.Chart._Data[this.Index] == null)
                    this.Chart._Data[this.Index] = new List<Entry>();

                var entries     = this.Chart._Data[this.Index];
                var entryIndex  = entries.Count;
                entries.Add(entry);

                return entryIndex;
            }

            public Boolean Equals(Position other) => (this.Chart == other.Chart) && (this.Index == other.Index);
            public override Boolean Equals(Object? other) => other is Position pos && this.Equals(pos);
            public override Int32 GetHashCode() => this.Chart.HashWithOther(this.Index);
            public override String ToString() => this.Index.ToString();

        }

    }

}
