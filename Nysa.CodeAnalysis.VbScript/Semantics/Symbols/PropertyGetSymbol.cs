using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed record PropertyGetSymbol : FunctionSymbol
    {
        public PropertyGetSymbol(String name, Boolean isPublic, Option<String> typeName, IEnumerable<Symbol> members, params String[] tags)
            : base(name, Option.None, Option.None, isPublic, typeName, members, tags)
        {
        }

        public override PropertyGetSymbol WithType(String typeName)
            => new PropertyGetSymbol(this.Name, this.IsPublic, typeName.Some(), this.Members);

        public override FunctionSymbol Renamed(String newName)
            => new PropertyGetSymbol(newName, this.IsPublic, this.TypeName, this.Members);
        
    }

}

