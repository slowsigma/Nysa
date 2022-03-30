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
        public IReadOnlySet<String>        Tags    { get; private set; }
        public IReadOnlyList<Symbol>       Members { get; private set; }
        public IDictionary<String, Symbol> Index   { get; private set; }

        protected ScriptSymbols(IEnumerable<Symbol> members, String[]? tags)
        {
            var parts = Symbols.Distinct(members);

            this.Tags       = new HashSet<String>((tags ?? new String[] { }), StringComparer.OrdinalIgnoreCase);
            this.Members    = parts.Members;
            this.Index      = parts.Index;
        }
    }

}