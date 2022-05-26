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
        public IReadOnlyList<Symbol>            Members   { get; init; }
        public IDictionary<String, Symbol>      Index     { get; init; }
        public IReadOnlyList<ArgumentSymbol>    Arguments { get; init; }

        private protected FunctionSymbol(String name, Option<String> newName, Option<String> message, Boolean isPublic, Option<String> typeName, IEnumerable<Symbol> members, IReadOnlySet<String> tags)
            : base(name, newName, message, isPublic, typeName, tags)
        {
            var parts = Symbols.Distinct(members);

            this.Members    = parts.Members;
            this.Index      = parts.Index;
            this.Arguments  = members.Where(m => m is ArgumentSymbol)
                                     .Cast<ArgumentSymbol>()
                                     .ToList();
        }

        public FunctionSymbol(String name, Option<String> newName, Option<String> message, Boolean isPublic, Option<String> typeName, IEnumerable<Symbol> members, params String[] tags)
            : this(name, newName, message, isPublic, typeName, members, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }

        public virtual FunctionSymbol Renamed(String newName)
            => new FunctionSymbol(this.Name, newName.Some(), Option.None, this.IsPublic, this.TypeName, this.Members, this.Tags);

        public override FunctionSymbol WithType(String typeName)
            => new FunctionSymbol(this.Name, this.NewName, this.Message, this.IsPublic, typeName.Some(), this.Members, this.Tags);
    }

}

