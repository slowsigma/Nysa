using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class PageSymbols : ScriptSymbols
    {
        public PageSymbols(IEnumerable<Symbol> members, String[]? tags = null)
            : base(members, tags)
        {
        }
    }

}