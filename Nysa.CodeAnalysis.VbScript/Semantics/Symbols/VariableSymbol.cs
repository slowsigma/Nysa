using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed record VariableSymbol : MemberSymbol
    {
        private VariableSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> typeName, IReadOnlySet<String> tags)
            : base(name, newName, errMessage, isPublic, typeName, tags)
        {
        }

        public VariableSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> typeName, params String[] tags)
            : this(name, newName, errMessage, isPublic, typeName, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }

        public VariableSymbol Renamed(String newName)
            => new VariableSymbol(this.Name, newName.Some(), Option.None, this.IsPublic, this.TypeName, this.Tags);

        public override VariableSymbol WithType(String typeName)
            => new VariableSymbol(this.Name, this.NewName, this.ErrMessage, this.IsPublic, typeName.Some(), this.Tags);
    }

}

