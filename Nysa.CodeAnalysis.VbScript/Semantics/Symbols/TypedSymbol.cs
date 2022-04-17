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

        private protected TypedSymbol(String name, Option<String> newName, Option<String> errMessage, Option<String> typeName, IReadOnlySet<String> tags)
            : base(name, newName, errMessage, tags)
        {
            this.TypeName = typeName;
        }

        protected TypedSymbol(String name, Option<String> newName, Option<String> errMessage, Option<String> typeName, String[]? tags = null)
            : this(name, newName, errMessage, typeName, new HashSet<String>(tags ?? new String[] { }, StringComparer.OrdinalIgnoreCase))
        {
        }

        public abstract TypedSymbol WithType(String typeName);
    }

}

