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
        public IDictionary<String, Symbol> Index   { get; private set; }

        protected ScriptSymbols(IEnumerable<Symbol> members)
        {
            var parts = Symbols.Distinct(members);

            this.Members    = parts.Members;
            this.Index      = parts.Index;
        }
    }

}