using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract record MemberSymbol : TypedSymbol
    {
        public Boolean IsPublic { get; private set; }

        protected MemberSymbol(String name, Option<String> newName, Option<String> message, Boolean isPublic, Option<String> typeName, IReadOnlySet<String> tags)
            : base(name, newName, message, typeName, tags)
        {
            this.IsPublic = isPublic;
        }

        protected MemberSymbol(String name, Option<String> newName, Option<String> message, Boolean isPublic, Option<String> typeName, params String[] tags)
            : this(name, newName, message, isPublic, typeName, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }
    }

}

