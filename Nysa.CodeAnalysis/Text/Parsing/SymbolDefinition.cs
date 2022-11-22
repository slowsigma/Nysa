using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Text.Parsing;

internal struct SymbolDefinition
{
    public static SymbolDefinition CreateTerminal(String symbol)
        => new SymbolDefinition(symbol, Grammar.TERMINAL);
    public static SymbolDefinition Create(SymbolIndex index, String symbol, IEnumerable<String[]> definitions)
        => new SymbolDefinition(symbol, definitions.Select(d => new GrammarRule(index, index.Id(symbol), d.Select(e => index.Id(e)))).ToList());


    // instance members
    public String                       Name        { get; private set; }
    public IReadOnlyList<GrammarRule>   Variants    { get; private set; }
    public Boolean                      IsTerminal  => this.Variants == Grammar.TERMINAL;

    private SymbolDefinition(String name, List<GrammarRule> variants)
    {
        this.Name       = name;
        this.Variants   = variants;
    }
}
