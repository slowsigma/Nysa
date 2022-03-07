using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record PropertyGetSymbol : FunctionSymbol
    {
        public PropertyGetSymbol(String name, Boolean isPublic, IEnumerable<Symbol> members)
            : base(name, Option.None, Option.None, isPublic, members)
        {
        }
    }

}

