using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed record PropertySetSymbol : FunctionSymbol
    {
        public Boolean IsLet { get; private set; }

        private PropertySetSymbol(String name, Boolean isPublic, Option<String> typeName, Boolean isLet, IEnumerable<Symbol> members, IReadOnlySet<String> tags)
            : base(name, Option.None, Option.None, isPublic, typeName, members, tags)
        {
            this.IsLet = isLet;
        }

        public PropertySetSymbol(String name, Boolean isPublic, Option<String> typeName, Boolean isLet, IEnumerable<Symbol> members, params String[] tags)
            : this(name, isPublic, typeName, isLet, members, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }

        public override PropertySetSymbol WithType(String typeName)
            => new PropertySetSymbol(this.Name, this.IsPublic, typeName.Some(), this.IsLet, this.Members, this.Tags);

        public override PropertySetSymbol Renamed(String newName)
            => new PropertySetSymbol(newName, this.IsPublic, this.TypeName, this.IsLet, this.Members, this.Tags);
    }

}

