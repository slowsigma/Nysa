using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract class ScriptSymbols : ISymbolScope
    {
        public IReadOnlyList<Symbol>       Members { get; private set; }
        public IDictionary<String, Symbol> Indexed { get; private set; }

        protected ScriptSymbols(IEnumerable<Symbol> members)
        {
            this.Members = members.ToList();
            this.Indexed = new ReadOnlyDictionary<String, Symbol>(members.ToDictionary(k => k.LookupKey(), StringComparer.OrdinalIgnoreCase));
        }
    }

}