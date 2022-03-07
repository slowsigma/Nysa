using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record VariableSymbol : MemberSymbol
    {
        public Option<String>   ClassName   { get; private set; }

        public VariableSymbol(String name, Option<String> newName, Option<String> errMessage, Boolean isPublic, Option<String> className)
            : base(name, newName, errMessage, isPublic)
        {
            this.ClassName = className;
        }

        public VariableSymbol Renamed(String newName)
            => new VariableSymbol(this.Name, newName.Some(), Option.None, this.IsPublic, this.ClassName);
    }

}

