using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Text.Parsing;

internal sealed class SymbolIndex
{
    private Dictionary<String, Identifier> _ToId;
    private Dictionary<Identifier, String> _ToSymbol;

    public SymbolIndex(IEnumerable<(String Symbol, Identifier Id)> items)
    {
        this._ToId      = items.ToDictionary(k => k.Symbol, v => v.Id, StringComparer.OrdinalIgnoreCase);
        this._ToSymbol  = _ToId.ToDictionary(k => k.Value, v => v.Key);
    }

    public Identifier Id(String symbol)
    {
        Identifier id;
        return this._ToId.TryGetValue(symbol, out id) ? id : Identifier.None;
    }

    public IEnumerable<(String Symbol, Identifier Id)> All => this._ToId.Select(kvp => (kvp.Key, kvp.Value));

    public String Symbol(Identifier id) => (id != Identifier.None && this._ToSymbol.ContainsKey(id)) ? this._ToSymbol[id] : "invalid-symbol-id";
} 