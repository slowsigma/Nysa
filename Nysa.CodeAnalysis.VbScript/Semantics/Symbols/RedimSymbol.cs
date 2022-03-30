using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record RedimSymbol : MemberSymbol
    {
        public RedimSymbol(String name, Option<String> newName, Option<String> errMessage, Option<String> typeName)
            : base(name, newName, errMessage, true, typeName)
        {
        }

        public RedimSymbol Renamed(String newName)
            => new RedimSymbol(this.Name, newName.Some(), Option.None, this.TypeName);

        public override RedimSymbol WithType(String typeName)
            => new RedimSymbol(this.Name, this.NewName, this.ErrMessage, typeName.Some());
    }

}

