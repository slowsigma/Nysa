using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Parsing;

public class GrammarBuilder
{
    // worker class
    private class RuleStart : IGrammarBuildStart, IGrammarBuildContinue
    {
        private Action<String[]> _Add;
        public RuleStart(Action<IEnumerable<String>> add)
        {
            this._Add = add;
        }

        public IGrammarBuildContinue Is(params String[] definition)
        {
            this._Add(definition);
            return this;
        }

        public IGrammarBuildContinue Or(params String[] definition) => this.Is(definition);
        public void OrOptional() => this.Is();
    }

    // instance members    
    public String StartSymbol { get; private set; }
    private Dictionary<String, (String Symbol, List<String[]> Definitions)> _Rules;

    public GrammarBuilder(String startSymbol)
    {
        this.StartSymbol = startSymbol;
        this._Rules = new Dictionary<String, (String Symbol, List<String[]> Definitions)>();
    }

    private void Add(String symbol, IEnumerable<String> definition)
    {
        var key = symbol.ToLowerInvariant();

        if (!this._Rules.ContainsKey(key))
            this._Rules.Add(key, (symbol, new List<String[]>()));

        this._Rules[key].Definitions.Add(definition.Where(i => i != null).ToArray());
    }

    public IGrammarBuildStart Rule(String symbol)
        => new RuleStart(d => this.Add(symbol, d));

    public IEnumerable<(String Symbol, List<String[]> Definitions)> Rules()
        => this._Rules.Select(kvp => kvp.Value);

    public IEnumerable<String> Symbols()
        => this._Rules.Select(r => r.Value.Symbol)
               .Concat(this._Rules.SelectMany(r => r.Value.Definitions.SelectMany(d => d)))
               .Distinct(StringComparer.OrdinalIgnoreCase);
}