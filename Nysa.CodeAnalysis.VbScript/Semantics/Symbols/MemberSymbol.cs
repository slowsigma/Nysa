using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract record MemberSymbol : HardSymbol
    {
        public Boolean IsPublic { get; private set; }

        protected MemberSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic)
            : base(name, newName, errMessage)
        {
            this.IsPublic  = isPublic;
        }
    }

}

