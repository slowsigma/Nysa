using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    public partial class Chart
    {

        public record struct Entry
        {
            public Grammar.Rule Rule { get; init; }
            public Int32 Number { get; init; }
            public Int32 Next { get; init; }

            public Entry(Grammar.Rule rule, Int32 number, Int32 next)
            {
                this.Rule       = rule;
                this.Number     = number;
                this.Next       = next;
            }

            public Identifier NextRuleId
            { get => (this.Next < this.Rule.DefinitionIds.Count) ? this.Rule.DefinitionIds[this.Next] : Identifier.None; }

            public Entry AsNextEntry() => new Entry(this.Rule, this.Number, this.Next + 1);
            public Entry AsInverted(Position fromPosition) => new Entry(this.Rule, fromPosition.Index - this.Number, this.Next);

            private IEnumerable<String> RuleState()
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
                return String.Concat(this.Rule.Symbol, " [", this.Number, "] ::= ", String.Join(" ", this.RuleState().ToArray()));
            }

        }

    }

}
