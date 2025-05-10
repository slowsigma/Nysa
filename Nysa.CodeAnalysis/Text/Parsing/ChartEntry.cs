using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Parsing;

public struct ChartEntry : IEquatable<ChartEntry>
{
    // static members
    public static Boolean operator ==(ChartEntry lhs, ChartEntry rhs) => lhs.Equals(rhs);
    public static Boolean operator !=(ChartEntry lhs, ChartEntry rhs) => !lhs.Equals(rhs);

    // instance members
    public GrammarRule  Rule    { get; private set; }
    public Int32        Number  { get; private set; }
    public Int32        Next    { get; private set; }

    public ChartEntry(GrammarRule rule, Int32 number, Int32 next)
    {
        this.Rule   = rule;
        this.Number = number;
        this.Next   = next;
    }

    public Identifier NextRuleId => (this.Next < this.Rule.DefinitionIds.Count) ? this.Rule.DefinitionIds[this.Next] : Identifier.None;
    public Boolean IsComplete => !(this.Next < this.Rule.DefinitionIds.Count);

    public ChartEntry AsNextEntry() => new ChartEntry(this.Rule, this.Number, this.Next + 1);
    public ChartEntry AsInverted(Int32 fromPosition) => new ChartEntry(this.Rule, fromPosition - this.Number, this.Next);

    public override bool Equals(object? obj) => obj is ChartEntry entry ? this.Equals(entry) : false;
    public bool Equals(ChartEntry other) => (this.Rule == other.Rule) && (this.Number == other.Number) && (this.Next == other.Next);
    public override int GetHashCode() => this.Rule.HashWithOther(this.Number, this.Next);

    public IEnumerable<String> RuleState()
    {
        Int32 dot = this.Next;
        foreach (var symbol in this.Rule.DefinitionSymbols())
        {
            if (dot-- == 0)
                yield return "¤";

            if (!String.IsNullOrEmpty(symbol))
                yield return symbol;
        }
    }

    public override string ToString()
    {
        var state = string.Join(" ", RuleState().ToArray());

        return $"{this.Rule.Symbol} [{this.Number}] ::= {state}";
    }

}
