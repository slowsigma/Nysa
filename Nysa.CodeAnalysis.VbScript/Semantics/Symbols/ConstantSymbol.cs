using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record ConstantSymbol : MemberSymbol
    {
        public ConstantSymbol(String name, Option<String> newName, Boolean isPublic)
            : base(name, newName, isPublic)
        {
        }

        public ConstantSymbol(String name, Boolean isPublic)
            : this(name, Option.None, isPublic)
        {
        }

        public ConstantSymbol Renamed(String newName)
            => new ConstantSymbol(this.Name, newName.Some(), this.IsPublic);
    }

}

