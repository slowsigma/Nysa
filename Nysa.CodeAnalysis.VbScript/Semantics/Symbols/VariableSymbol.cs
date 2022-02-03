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

        public VariableSymbol(String name, Option<String> newName, Boolean isPublic, Option<String> className)
            : base(name, newName, isPublic)
        {
            this.ClassName = className;
        }

        public VariableSymbol(String name, Boolean isPublic, ClassSymbol? @class = null)
            : this(name, Option.None, isPublic, @class == null ? Option.None : @class.Name.Some())
        {
        }

        public VariableSymbol Renamed(String newName)
            => new VariableSymbol(this.Name, newName.Some(), this.IsPublic, this.ClassName);
    }

}

