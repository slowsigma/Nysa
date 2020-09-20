using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {
        // static members
        public sealed class RuleVariant
        {
            public Identifier Id { get; private set; }
            public String Symbol(Grammar grammar) => grammar.GetSymbol(this.Id);
            public IReadOnlyList<Identifier> DefinitionIds { get; private set; }
            public Boolean IsEmpty => this.DefinitionIds.Count == 0;

            public RuleVariant(Identifier id, IEnumerable<Identifier> definitionIds)
            {
                this.Id             = id;
                this.DefinitionIds  = new List<Identifier>(definitionIds);
            }

            public IEnumerable<String> DefinitionSymbols(Grammar grammar)
                => this.DefinitionIds.Select(id => grammar.GetSymbol(id));
            public string ToString(Grammar grammar)
                => String.Concat(grammar.GetSymbol(this.Id), " ::= ", String.Join(" ", this.DefinitionSymbols(grammar)));

        } // class Rule

    }

}
