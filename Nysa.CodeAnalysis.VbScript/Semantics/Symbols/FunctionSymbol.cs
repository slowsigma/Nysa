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

        public FunctionSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, IEnumerable<Symbol> members)
            : base(name, newName, errMessage, isPublic)
        {
            var parts = Symbols.Distinct(members);

            this.Members  = parts.Members;
            this.Index    = parts.Index;
        }

        public FunctionSymbol Renamed(String newName)
            => new FunctionSymbol(this.Name, newName.Some(), Option.None, this.IsPublic, this.Members);
    }

}

