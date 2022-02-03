using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record ArgumentSymbol : HardSymbol
    {
        public ArgumentSymbol(String name, Option<String> newName)
            : base(name, newName)
        {
        }

        public ArgumentSymbol(String name)
            : this(name, Option.None)
        {
        }

        public ArgumentSymbol Renamed(String newName)
            => new ArgumentSymbol(this.Name, newName.Some());
    }

}

