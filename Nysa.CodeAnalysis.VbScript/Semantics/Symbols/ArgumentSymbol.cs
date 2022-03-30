using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record ArgumentSymbol : TypedSymbol
    {
        public Boolean IsByRef { get; private set; }

        private ArgumentSymbol(String name, Option<String> newName, Option<String> errMessage, Option<String> typeName, Boolean isByRef = true)
            : base(name, newName, errMessage, typeName)
        {
            this.IsByRef = isByRef;
        }

        public ArgumentSymbol(String name, Option<String> newName, Option<String> typeName, Boolean isByRef = true)
            : this(name, newName, Option.None, typeName, isByRef)
        {
        }

        public ArgumentSymbol(String name, Boolean isByRef = true)
            : this(name, Option.None, Option.None, Option.None, isByRef)
        {
        }

        public ArgumentSymbol Renamed(String newName)
            => new ArgumentSymbol(this.Name, newName.Some(), this.ErrMessage, this.TypeName, this.IsByRef);

        public override ArgumentSymbol WithType(String typeName)
            => new ArgumentSymbol(this.Name, this.NewName, this.ErrMessage, typeName.Some(), this.IsByRef);
    }

}

