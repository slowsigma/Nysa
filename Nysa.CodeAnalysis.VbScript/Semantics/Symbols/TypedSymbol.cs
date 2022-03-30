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
        protected TypedSymbol(String name, Option<String> newName, Option<String> errMessage, Option<String> typeName)
            : base(name, newName, errMessage)
        {
            this.TypeName = typeName;
        }

        public abstract TypedSymbol WithType(String typeName);
    }

}

