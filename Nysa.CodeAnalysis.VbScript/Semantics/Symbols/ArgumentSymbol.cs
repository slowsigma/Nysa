using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record ArgumentSymbol : HardSymbol
    {
        public Boolean IsByRef { get; private set; }
        public ArgumentSymbol(String name, Option<String> newName, Boolean isByRef = true)
            : base(name, newName, Option.None)
        {
            this.IsByRef = isByRef;
        }

        public ArgumentSymbol(String name, Boolean isByRef = true)
            : this(name, Option.None, isByRef)
        {
        }

        public ArgumentSymbol Renamed(String newName)
            => new ArgumentSymbol(this.Name, newName.Some(), this.IsByRef);
    }

}

