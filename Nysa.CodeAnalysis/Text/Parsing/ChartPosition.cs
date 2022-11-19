using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Parsing;

public struct ChartPosition : IEquatable<ChartPosition>, IEnumerable<ChartEntry>
{
    // static members
    public static Boolean operator ==(ChartPosition lhs, ChartPosition rhs) => lhs.Equals(rhs);
    public static Boolean operator !=(ChartPosition lhs, ChartPosition rhs) => !lhs.Equals(rhs);

    // instance members
    public Chart Chart { get; private set; }
    public Int32 Index { get; private set; }
    
    public ChartPosition(Chart chart, Int32 index)
    {
        this.Chart = chart;
        this.Index = index;
    }

    public Int32 Count { get => this.Chart._Data[this.Index] == null ? 0 : this.Chart._Data[this.Index].Count; }
    public ChartEntry this[Int32 entry] { get => this.Chart._Data[this.Index][entry]; }

    public IEnumerable<ChartEntry> Where(Func<ChartEntry, Boolean> predicate)
        => (this.Chart._Data[this.Index] != null)
            ? this.Chart._Data[this.Index].Where(predicate)
            : None<ChartEntry>.Enumerable();

    public IEnumerable<ChartEntry> GetEntries()
    {
        if (this.Chart._Data[this.Index] == null)
            yield break;

        for (Int32 e = 0; e < this.Chart._Data[this.Index].Count; e++)
            yield return this.Chart._Data[this.Index][e];
    }

    public IEnumerator<ChartEntry> GetEnumerator()
    {
        if (this.Chart._Data[this.Index] == null)
            yield break;

        for (Int32 e = 0; e < this.Chart._Data[this.Index].Count; e++)
            yield return this.Chart._Data[this.Index][e];
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    internal Int32 AddUnique(ChartEntry entry)
    {
        if (this.Chart._Data[this.Index] == null)
            this.Chart._Data[this.Index] = new List<ChartEntry>();

        var entries = this.Chart._Data[this.Index];
        var entryIndex = entries.IndexOf(entry);

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
            this.Chart._Data[this.Index] = new List<ChartEntry>();

        var entries = this.Chart._Data[this.Index];
        var entry = new ChartEntry(rule, origin, next);
        var entryIndex = entries.IndexOf(entry);

        if (entryIndex < 0)
        {
            entryIndex = entries.Count;
            entries.Add(entry);
        }

        return entryIndex;
    }

    internal Int32 AddRaw(ChartEntry entry)
    {
        if (this.Chart._Data[this.Index] == null)
            this.Chart._Data[this.Index] = new List<ChartEntry>();

        var entries = this.Chart._Data[this.Index];
        var entryIndex = entries.Count;
        entries.Add(entry);

        return entryIndex;
    }

    public Boolean Equals(ChartPosition other) => (this.Chart == other.Chart) && (this.Index == other.Index);
    public override Boolean Equals(Object? other) => other is ChartPosition pos && this.Equals(pos);
    public override Int32 GetHashCode() => this.Chart.HashWithOther(this.Index);
    public override String ToString() => this.Index.ToString();

}
