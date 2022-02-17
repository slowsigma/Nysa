using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record SymbolLayer
    {
        public Option<SymbolLayer>          Up       { get; init; }
        public ISymbolScope                 Value    { get; init; }
        public IReadOnlyList<Symbol>        Members  => this.Value.Members;
        public IDictionary<String, Symbol>  Index    => this.Value.Index;

        public SymbolLayer(Option<SymbolLayer> up, ISymbolScope value)
        {
            this.Up     = up;
            this.Value  = value;
        }

        public Option<SymbolLayer> MemberLayer(String symbolName)
            =>    this.Index.ContainsKey(symbolName)
               && this.Index[symbolName] is ISymbolScope scope
               ? (new SymbolLayer(this.Some(), scope)).Some()
               : Option<SymbolLayer>.None;
    }

}

