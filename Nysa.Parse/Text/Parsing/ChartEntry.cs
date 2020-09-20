using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    public struct ChartEntry : IEquatable<ChartEntry>
    {
        // static members
        public static Boolean operator ==(ChartEntry lhs, ChartEntry rhs) => lhs.Equals(rhs);
        public static Boolean operator !=(ChartEntry lhs, ChartEntry rhs) => !lhs.Equals(rhs);

        // instance members
        public Grammar.RuleVariant  Rule    { get; private set; }
        public Int32                Number  { get; private set; }
        public Int32                Next    { get; private set; }

        public ChartEntry(Grammar.RuleVariant rule, Int32 number, Int32 next)
        {
            this.Rule       = rule;
            this.Number     = number;
            this.Next       = next;
        }

        public Identifier NextRuleId
            => (this.Next < this.Rule.DefinitionIds.Count) ? this.Rule.DefinitionIds[this.Next] : Identifier.None;

        public ChartEntry AsNextEntry() => new ChartEntry(this.Rule, this.Number, this.Next + 1);
        public ChartEntry AsInverted(ChartPosition fromPosition) => new ChartEntry(this.Rule, fromPosition.Index - this.Number, this.Next);

        public override Boolean Equals(object obj) => (obj is ChartEntry) ? this.Equals((ChartEntry)obj) : false;
        public Boolean Equals(ChartEntry other) => (this.Rule == other.Rule) && (this.Number == other.Number) && (this.Next == other.Next);
        public override Int32 GetHashCode() => this.Rule.HashWithOther(this.Number, this.Next);

        private IEnumerable<String> RuleState(Grammar grammar)
        {
            Int32 dot = this.Next;
            foreach (var symbol in this.Rule.DefinitionSymbols(grammar))
            {
                if (dot-- == 0)
                    yield return "¤";

                if (!String.IsNullOrEmpty(symbol))
                    yield return symbol;
            }
        }

        public String ToString(Grammar grammar)
            => String.Concat(this.Rule.Symbol(grammar), " [", this.Number, "] ::= ", String.Join(" ", this.RuleState(grammar).ToArray()));

    }

}
