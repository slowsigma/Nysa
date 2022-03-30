using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record VariableSymbol : MemberSymbol
    {
        public VariableSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> typeName)
            : base(name, newName, errMessage, isPublic, typeName)
        {
        }

        public VariableSymbol Renamed(String newName)
            => new VariableSymbol(this.Name, newName.Some(), Option.None, this.IsPublic, this.TypeName);

        public override VariableSymbol WithType(String typeName)
            => new VariableSymbol(this.Name, this.NewName, this.ErrMessage, this.IsPublic, typeName.Some());
    }

}

