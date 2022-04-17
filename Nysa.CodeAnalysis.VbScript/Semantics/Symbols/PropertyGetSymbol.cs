using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed record PropertyGetSymbol : FunctionSymbol
    {
        private PropertyGetSymbol(String name, Boolean isPublic, Option<String> typeName, IEnumerable<Symbol> members, IReadOnlySet<String> tags)
            : base(name, Option.None, Option.None, isPublic, typeName, members, tags)
        {
        }

        public PropertyGetSymbol(String name, Boolean isPublic, Option<String> typeName, IEnumerable<Symbol> members, params String[] tags)
            : this(name, isPublic, typeName, members, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }

        public override PropertyGetSymbol WithType(String typeName)
            => new PropertyGetSymbol(this.Name, this.IsPublic, typeName.Some(), this.Members, this.Tags);

        public override FunctionSymbol Renamed(String newName)
            => new PropertyGetSymbol(newName, this.IsPublic, this.TypeName, this.Members, this.Tags);
        
    }

}

