using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Text.Parsing;

internal struct SymbolDefinition
{
    public static SymbolDefinition CreateTerminal(String symbol)
        => new SymbolDefinition(symbol, NodePolicy.Default, Grammar.TERMINAL);
    public static SymbolDefinition Create(SymbolIndex index, String symbol, NodePolicy nodePolicy, IEnumerable<String[]> definitions)
        => new SymbolDefinition(symbol, nodePolicy, definitions.Select(d => new GrammarRule(index, index.Id(symbol), d.Select(e => index.Id(e)))).ToList());


    // instance members
    public String                       Name        { get; init; }
    public NodePolicy                   NodePolicy  { get; init; }
    public IReadOnlyList<GrammarRule>   Variants    { get; init; }
    public Boolean                      IsTerminal  => this.Variants == Grammar.TERMINAL;

    private SymbolDefinition(String name, NodePolicy nodePolicy, List<GrammarRule> variants)
    {
        this.Name       = name;
        this.NodePolicy = nodePolicy;
        this.Variants   = variants;
    }
}
