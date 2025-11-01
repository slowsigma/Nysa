using System;

namespace Nysa.CodeAnalysis.VbScript.Semantics;

public sealed record BaseSymbolMatch(
    Symbol Symbol,
    SymbolLayer Scope
) : SymbolMatch(Symbol, Scope)
{
    public override SymbolMatch WithSymbol(Symbol symbol)
        => new BaseSymbolMatch(symbol, this.Scope);
}

public sealed record BaseSymbolMatch<T>(
   T Symbol,
   SymbolLayer Scope
) where T : Symbol;
