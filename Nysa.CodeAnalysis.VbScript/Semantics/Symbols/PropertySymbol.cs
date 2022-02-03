using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record PropertySymbol : Symbol
    {
        public Option<FunctionSymbol> Getter    { get; private set; }
        public Option<FunctionSymbol> Setter    { get; private set; }

        public PropertySymbol(String name, Option<FunctionSymbol> getter, Option<FunctionSymbol> setter)
            : base(name)
        {
            if (getter is Some<FunctionSymbol> someGet && someGet.Value is PropertySetSymbol)
                throw new Exception("PropertySymbol getter cannot be of type PropertySetSymbol.");
            if (setter is Some<FunctionSymbol> someSet && someSet.Value is PropertyGetSymbol)
                throw new Exception("PropertySymbol setter cannot be of type PropertyGetSymbol.");

            this.Getter = getter;
            this.Setter = setter;
        }
    }

}

