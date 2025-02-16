using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing;

public static class GrammarFunctions
{

    public static Grammar ToGrammar(this GrammarBuilder @this)
    {
        // this.StartSymbol = @this.StartSymbol;
        var symbolIndex = new SymbolIndex(@this.Symbols()
                                               .Select((s, i) => (Symbol: s, Id: Identifier.FromNumber(i + 1))));

        var startId     = symbolIndex.Id(@this.StartSymbol);

        if (startId == Identifier.None)
            throw new Exception($"Cannot find start symbol '{@this.StartSymbol}' in the grammar's definition.");

        var definitions = @this.Rules()
                               .ToDictionary(rk => rk.Symbol, 
                                             rv => SymbolDefinition.Create(symbolIndex, rv.Symbol, rv.NodePolicy, rv.Definitions),
                                             StringComparer.OrdinalIgnoreCase);

        var rules       = symbolIndex.All
                                     .Select(kvp => definitions.ContainsKey(kvp.Symbol)
                                                    ? definitions[kvp.Symbol]
                                                    : SymbolDefinition.CreateTerminal(kvp.Symbol))
                                     .ToDictionary(dk => symbolIndex.Id(dk.Name));

        if (definitions[@this.StartSymbol].NodePolicy != NodePolicy.Default)
            throw new Exception($"The start symbol '{@this.StartSymbol}' cannot have a node policy other than Default.");

        var known       = new HashSet<Identifier>(rules.Where(r => r.Value.IsTerminal).Select(t => t.Key));
        var nullable    = new HashSet<Identifier>(rules.Where(r => !r.Value.IsTerminal && r.Value.Variants.Any(v => v.IsEmpty)).Select(n => n.Key));
        var unknown     = new HashSet<Identifier>(rules.Where(r => !known.Contains(r.Key) && !nullable.Contains(r.Key)).Select(u => u.Key));

        var moreNull = unknown.Where(u => !rules[u].IsTerminal && rules[u].Variants.Any(d => d.DefinitionIds.All(s => nullable.Contains(s)))).ToArray();

        while (moreNull.Length > 0)
        {
            nullable.UnionWith(moreNull);
            unknown.ExceptWith(moreNull);
            moreNull = unknown.Where(u => !rules[u].IsTerminal && rules[u].Variants.Any(d => d.DefinitionIds.All(s => nullable.Contains(s)))).ToArray();
        }

        return new Grammar(@this.StartSymbol, startId, symbolIndex, rules, nullable);
    }

}
