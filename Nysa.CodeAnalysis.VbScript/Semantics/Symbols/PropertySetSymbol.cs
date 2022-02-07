using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record PropertySetSymbol : FunctionSymbol
    {
        public Boolean IsLet { get; private set; }
        public PropertySetSymbol(String name, Boolean isPublic, Boolean isLet, IEnumerable<Symbol> members)
            : base(name, Option.None, isPublic, members)
        {
            this.IsLet = isLet;
        }
    }

}

