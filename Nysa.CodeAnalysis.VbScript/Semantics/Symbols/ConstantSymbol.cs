using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record ConstantSymbol : MemberSymbol
    {
        public ConstantSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic)
            : base(name, newName, errMessage, isPublic)
        {
        }

        public ConstantSymbol Renamed(String newName)
            => new ConstantSymbol(this.Name, newName.Some(), Option.None, this.IsPublic);
    }

}

