using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed record RedimSymbol : MemberSymbol
    {
        public RedimSymbol(String name, Option<String> newName, Option<String> message, Option<String> typeName)
            : base(name, newName, message, true, typeName)
        {
        }

        public RedimSymbol Renamed(String newName)
            => new RedimSymbol(this.Name, newName.Some(), Option.None, this.TypeName);

        public override RedimSymbol WithType(String typeName)
            => new RedimSymbol(this.Name, this.NewName, this.Message, typeName.Some());
    }

}

