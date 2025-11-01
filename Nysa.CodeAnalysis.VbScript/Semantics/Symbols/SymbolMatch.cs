using System;

namespace Nysa.CodeAnalysis.VbScript.Semantics;

public abstract record SymbolMatch<T>(
    T Symbol,
    SymbolLayer Scope
) where T : Symbol;

public abstract record SymbolMatch(
   Symbol Symbol,
   SymbolLayer Scope
) : SymbolMatch<Symbol>(Symbol, Scope)
{
    public abstract SymbolMatch WithSymbol(Symbol symbol);
}