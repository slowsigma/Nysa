using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record BlockSymbol : Symbol, ISymbolScope
    {
        public IReadOnlySet<String>        Tags     { get; private set; }
        public IReadOnlyList<Symbol>       Members  { get; private set; }
        public IDictionary<String, Symbol> Index    { get; private set; }

        public BlockSymbol(String name, IEnumerable<Symbol> members)
            : base(name)
        {
            var parts = Symbols.Distinct(members);

            this.Tags       = new HashSet<String>();
            this.Members    = parts.Members;
            this.Index      = parts.Index;
        }
    }

}

