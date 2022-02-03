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

        protected MemberSymbol(String name, Option<String> newName, Boolean isPublic)
            : base(name, newName)
        {
            this.IsPublic  = isPublic;
        }

        public MemberSymbol(String name, Boolean isPublic)
            : this(name, Option.None, isPublic)
        {
        }
    }

}

