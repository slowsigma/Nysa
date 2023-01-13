using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract record TypedSymbol : HardSymbol
    {
        public Option<String> TypeName { get; private set; }

        protected TypedSymbol(String name, Option<String> newName, Option<String> message, Option<String> typeName, IReadOnlySet<String> tags)
            : base(name, newName, message, tags)
        {
            this.TypeName = typeName;
        }

        protected TypedSymbol(String name, Option<String> newName, Option<String> message, Option<String> typeName, String[]? tags = null)
            : this(name, newName, message, typeName, new HashSet<String>(tags ?? new String[] { }, StringComparer.OrdinalIgnoreCase))
        {
        }

        public abstract TypedSymbol WithType(String typeName);
    }

}

