using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {
        // static members
        public sealed class Rule
        {
            public Grammar Grammar { get; private set; }
            public Identifier Id { get; private set; }
            public String Symbol { get => this.Grammar.Symbol(this.Id); }
            public IReadOnlyList<Identifier> DefinitionIds { get; private set; }
            public Boolean IsEmpty { get { return this.DefinitionIds.Count == 0; } }

            public Rule(Grammar grammar, Identifier id, IEnumerable<Identifier> definitionIds)
            {
                this.Grammar        = grammar;
                this.Id             = id;
                this.DefinitionIds  = new List<Identifier>(definitionIds);
            }

            public IEnumerable<String> DefinitionSymbols()
                => this.DefinitionIds.Select(id => this.Grammar.Symbol(id));
            public override string ToString()
                => String.Concat(this.Grammar.Symbol(this.Id), " ::= ", String.Join(" ", this.DefinitionSymbols()));

        } // class Rule

    }

}
