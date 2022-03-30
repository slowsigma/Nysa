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

        protected MemberSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> typeName)
            : base(name, newName, errMessage, typeName)
        {
            this.IsPublic  = isPublic;
        }
    }

}

