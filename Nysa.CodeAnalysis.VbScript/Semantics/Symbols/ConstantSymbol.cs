using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record ConstantSymbol : MemberSymbol
    {
        public ConstantSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> typeName)
            : base(name, newName, errMessage, isPublic, typeName)
        {
        }

        public ConstantSymbol Renamed(String newName)
            => new ConstantSymbol(this.Name, newName.Some(), this.ErrMessage, this.IsPublic, this.TypeName);

        public override ConstantSymbol WithType(String typeName)
            => new ConstantSymbol(this.Name, this.NewName, this.ErrMessage, this.IsPublic, typeName.Some());
    }

}

