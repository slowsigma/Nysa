using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Text.Parsing
{

    public partial class Grammar
    {
        // static members
        private struct SymbolDefinition
        {
            // static members
            public static SymbolDefinition CreateTerminal(String symbol)
                => new SymbolDefinition(symbol, Grammar.TERMINAL);
            public static SymbolDefinition Create(Grammar grammar, String symbol, IEnumerable<String[]> definitions)
                => new SymbolDefinition(symbol, definitions.Select(d => new Rule(grammar, grammar.Id(symbol), d.Select(e => grammar.Id(e)))).ToList());

            // instance members
            public String Name { get; private set; }
            public String Key { get => this.Name.ToLowerInvariant(); }
            public List<Rule> Variants { get; private set; }
            public Boolean IsTerminal { get => this.Variants == TERMINAL; }

            private SymbolDefinition(String name, List<Rule> variants)
            {
                this.Name       = name;
                this.Variants   = variants;
            }

        }

    }

}
