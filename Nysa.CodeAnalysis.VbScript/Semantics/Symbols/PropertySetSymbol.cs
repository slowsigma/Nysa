using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record PropertySetSymbol : FunctionSymbol
    {
        public PropertySetSymbol(String name, Boolean isPublic, IEnumerable<Symbol> members)
            : base(name, Option.None, isPublic, members)
        {
        }
    }

}

