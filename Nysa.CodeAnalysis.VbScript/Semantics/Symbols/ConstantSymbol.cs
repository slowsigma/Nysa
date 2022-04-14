using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record ConstantSymbol : MemberSymbol
    {
        public LiteralValueTypes Type { get; private set; }

        public ConstantSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, LiteralValueTypes type)
            : base(name, newName, errMessage, isPublic, (Enum.GetName(type) ?? "Nothing").Some())
        {
            this.Type = type;
        }

        public ConstantSymbol Renamed(String newName)
            => new ConstantSymbol(this.Name, newName.Some(), this.ErrMessage, this.IsPublic, this.Type);

        public override ConstantSymbol WithType(String typeName)
            => new ConstantSymbol(this.Name, this.NewName, this.ErrMessage, this.IsPublic, this.Type);
    }

}

