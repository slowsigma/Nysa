using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract record HardSymbol : Symbol
    {
        public Option<String> NewName { get; private set; }

        protected HardSymbol(String name, Option<String> newName)
            : base(name)
        {
            this.NewName = newName;
        }

        protected HardSymbol(String name, String newName)
            : this(name, newName.Some())
        {
        }

        protected HardSymbol(String name)
            : this(name, Option<String>.None)
        {
        }
    }

}

