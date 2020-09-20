using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    public struct ChartPosition : IEquatable<ChartPosition>, IEnumerable<ChartEntry>
    {
        public static Boolean operator ==(ChartPosition lhs, ChartPosition rhs) => lhs.Equals(rhs);
        public static Boolean operator !=(ChartPosition lhs, ChartPosition rhs) => !lhs.Equals(rhs);

        // instance members
        public Chart  Chart { get; private set; }
        public Int32  Index { get; private set; }

        public ChartPosition(Chart chart, Int32 index)
        {
            this.Chart = chart;
            this.Index = index;
        }

        public Int32 Count { get => this.Chart.Raw(this.Index) == null ? 0 : this.Chart.Raw(this.Index).Count; }
        public ChartEntry this[Int32 entry] { get => this.Chart[this.Index][entry]; }

        public IEnumerable<ChartEntry> Where(Func<ChartEntry, Boolean> predicate)
            => (this.Chart.Raw(this.Index) != null)
               ? this.Chart.Raw(this.Index).Where(predicate)
               : Enumerable.Empty<ChartEntry>();

        public IEnumerable<ChartEntry> GetEntries()
        {
            var data = this.Chart.Raw(this.Index);

            if (data == null)
                yield break;

            for (Int32 e = 0; e < data.Count; e++)
                yield return data[e];
        }

        public IEnumerator<ChartEntry> GetEnumerator()
        {
            var data = this.Chart.Raw(this.Index);

            if (data == null)
                yield break;

            for (Int32 e = 0; e < data.Count; e++)
                yield return data[e];
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        internal Int32 AddUnique(ChartEntry entry)
        {
            var entries     = this.Chart.RawNotNull(this.Index);
            var entryIndex  = entries.IndexOf(entry);

            if (entryIndex < 0)
            {
                entryIndex = entries.Count;
                entries.Add(entry);
            }

            return entryIndex;
        }

        internal Int32 AddUnique(Grammar.RuleVariant rule, Int32 origin, Int32 next)
        {
            var entries     = this.Chart.RawNotNull(this.Index);
            var entry       = new ChartEntry(rule, origin, next);
            var entryIndex  = entries.IndexOf(entry);

            if (entryIndex < 0)
            {
                entryIndex = entries.Count;
                entries.Add(entry);
            }

            return entryIndex;
        }

        internal Int32 AddRaw(ChartEntry entry)
        {
            var entries     = this.Chart.RawNotNull(this.Index);
            var entryIndex  = entries.Count;
            entries.Add(entry);

            return entryIndex;
        }

        public Boolean Equals(ChartPosition other) => (this.Chart == other.Chart) && (this.Index == other.Index);
        public override Boolean Equals(Object other) => (other is ChartPosition) && this.Equals((ChartPosition)other);
        public override Int32 GetHashCode() => this.Chart.HashWithOther(this.Index);
        public override String ToString() => this.Index.ToString();

    }

}
