using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class CodeSymbols : ScriptSymbols
    {
        public CodeSymbols(IEnumerable<Symbol> members)
            : base(members)
        {
        }

        public IncludeSymbols ToIncludeSymbols()
            => new IncludeSymbols(this.Members);
    }

}