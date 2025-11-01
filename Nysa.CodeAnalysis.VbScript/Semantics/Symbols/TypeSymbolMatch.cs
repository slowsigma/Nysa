using System;

namespace Nysa.CodeAnalysis.VbScript.Semantics;

public sealed record TypeSymbolMatch(
    Symbol Symbol,
    SymbolLayer Scope,
    ClassSymbol Type
) : SymbolMatch(Symbol, Scope)
{
    public override SymbolMatch WithSymbol(Symbol symbol)
        => new TypeSymbolMatch(symbol, this.Scope, this.Type);
}
