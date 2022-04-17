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

        private protected MemberSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> typeName, IReadOnlySet<String> tags)
            : base(name, newName, errMessage, typeName, tags)
        {
            this.IsPublic = isPublic;
        }

        protected MemberSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> typeName, params String[] tags)
            : this(name, newName, errMessage, isPublic, typeName, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }
    }

}

