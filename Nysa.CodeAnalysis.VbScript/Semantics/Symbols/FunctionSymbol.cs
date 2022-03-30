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
        public IReadOnlySet<String>        Tags     { get; private set; }
        public IReadOnlyList<Symbol>       Members  { get; private set; }
        public IDictionary<String, Symbol> Index    { get; private set; }

        public FunctionSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> typeName, IEnumerable<Symbol> members)
            : base(name, newName, errMessage, isPublic, typeName)
        {
            var parts = Symbols.Distinct(members);

            this.Tags     = new HashSet<String>();
            this.Members  = parts.Members;
            this.Index    = parts.Index;
        }

        public virtual FunctionSymbol Renamed(String newName)
            => new FunctionSymbol(this.Name, newName.Some(), Option.None, this.IsPublic, this.TypeName, this.Members);

        public override FunctionSymbol WithType(String typeName)
            => new FunctionSymbol(this.Name, this.NewName, this.ErrMessage, this.IsPublic, typeName.Some(), this.Members);
    }

}

