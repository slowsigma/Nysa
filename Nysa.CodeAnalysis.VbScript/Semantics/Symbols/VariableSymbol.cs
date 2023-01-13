using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed record VariableSymbol : MemberSymbol
    {
        public VariableSymbol(String name, Option<String> newName, Option<String> message, Boolean isPublic, Option<String> typeName, IReadOnlySet<String> tags)
            : base(name, newName, message, isPublic, typeName, tags)
        {
        }

        public VariableSymbol(String name, Option<String> newName, Option<String> message, Boolean isPublic, Option<String> typeName, params String[] tags)
            : this(name, newName, message, isPublic, typeName, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }

        public VariableSymbol Renamed(String newName)
            => new VariableSymbol(this.Name, newName.Some(), Option.None, this.IsPublic, this.TypeName, this.Tags);

        public override VariableSymbol WithType(String typeName)
            => new VariableSymbol(this.Name, this.NewName, this.Message, this.IsPublic, typeName.Some(), this.Tags);
    }

}

