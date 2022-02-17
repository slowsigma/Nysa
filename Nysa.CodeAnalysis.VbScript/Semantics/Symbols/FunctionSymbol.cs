using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record FunctionSymbol : MemberSymbol, ISymbolScope
    {
        public IReadOnlyList<Symbol>       Members  { get; private set; }
        public IDictionary<String, Symbol> Index    { get; private set; }

        public FunctionSymbol(String name, Option<String> newName, Boolean isPublic, IEnumerable<Symbol> members)
            : base(name, newName, isPublic)
        {
            var parts = Symbols.Distinct(members);

            this.Members  = parts.Members;
            this.Index    = parts.Index;
        }

        public FunctionSymbol(String name, Boolean isPublic, IEnumerable<Symbol> members)
            : this(name, Option.None, isPublic, members)
        {
        }

        public FunctionSymbol Renamed(String newName)
            => new FunctionSymbol(this.Name, newName.Some(), this.IsPublic, this.Members);
    }

}

