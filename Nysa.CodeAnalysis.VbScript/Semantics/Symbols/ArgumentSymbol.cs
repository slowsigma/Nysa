using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed record ArgumentSymbol : TypedSymbol
    {
        public Boolean IsByRef { get; init; }
        public Boolean IsOptional { get; init; }

        public ArgumentSymbol(String name, Option<String> newName, Option<String> message, Option<String> typeName, Boolean isByRef, Boolean isOptional, IReadOnlySet<String> tags)
            : base(name, newName, message, typeName, tags)
        {
            this.IsByRef    = isByRef;
            this.IsOptional = isOptional;
        }

        public ArgumentSymbol(String name, Option<String> newName, Option<String> typeName, Boolean isByRef = true, Boolean isOptional = false, params String[] tags)
            : this(name, newName, Option.None, typeName, isByRef, isOptional, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }

        public ArgumentSymbol(String name, Boolean isByRef = true, Boolean isOptional = false, params String[] tags)
            : this(name, Option.None, Option.None, Option.None, isByRef, isOptional, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }

        public ArgumentSymbol Renamed(String newName)
            => new ArgumentSymbol(this.Name, newName.Some(), this.Message, this.TypeName, this.IsByRef, this.IsOptional, this.Tags);

        public override ArgumentSymbol WithType(String typeName)
            => new ArgumentSymbol(this.Name, this.NewName, this.Message, typeName.Some(), this.IsByRef, this.IsOptional, this.Tags);
    }

}

