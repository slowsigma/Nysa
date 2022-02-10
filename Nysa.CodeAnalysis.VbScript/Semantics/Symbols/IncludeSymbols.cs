using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class IncludeSymbols : ScriptSymbols
    {
        public IncludeSymbols(IEnumerable<Symbol> members)
            : base(members)
        {
        }
    }

}