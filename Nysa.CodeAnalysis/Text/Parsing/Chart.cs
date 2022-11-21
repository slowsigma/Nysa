using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Parsing;

public class Chart : IEnumerable<ChartPosition>
{
    public Grammar Grammar { get; private set; }

    internal List<ChartEntry>[] _Data;

    internal Chart(Grammar grammar, Int32 size)
    {
        this.Grammar    = grammar;
        this._Data      = new List<ChartEntry>[size];
    }

    public ChartPosition this[Int32 index]
    {
        get => (index >= 0) && (index < this._Data.Length)
               ? new ChartPosition(this, index)
               : throw new IndexOutOfRangeException();
    }

    internal void AddUnique(Int32 index, ChartEntry entry)
    {
        if (this._Data[index] == null)
            this._Data[index] = new List<ChartEntry>();

        var entries = this._Data[index];

        if (entries.IndexOf(entry) < 0)
            entries.Add(entry);
    }

    internal void AddUnique(Int32 index, GrammarRule rule, Int32 origin, Int32 next)
    {
        if (this._Data[index] == null)
            this._Data[index] = new List<ChartEntry>();

        var entries = this._Data[index];
        var entry = new ChartEntry(rule, origin, next);

        if (entries.IndexOf(entry) < 0)
            entries.Add(entry);
    }

    internal void AddRaw(Int32 index, ChartEntry entry)
    {
        if (this._Data[index] == null)
            this._Data[index] = new List<ChartEntry>();

        var entries = this._Data[index];

        entries.Add(entry);
    }

    public IEnumerator<ChartPosition> GetEnumerator()
    {
        foreach (var p in this._Data.Select((l, i) => new ChartPosition(this, i)))
            yield return p;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public Int32 Length { get => this._Data.Length; }

    public override string ToString()
    {
        var build = new StringBuilder();

        foreach (var position in this)
        {
            build.AppendLine($"[Position {position.Index}]");

            foreach (var entry in position.Where(e => true))
            {
                build.AppendLine(entry.ToString());
            }
        }

        return build.ToString();
    }

}
