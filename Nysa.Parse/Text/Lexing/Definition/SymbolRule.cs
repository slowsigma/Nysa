using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Logics;

namespace Nysa.Text.Lexing.Definition
{

    public class SymbolRule : Rule
    {
        public Identifier           Identifier  { get; private set; }
        public Option<AssertRule>   AssertPrior { get; private set; }
        public CaptureRule          Capture     { get; private set; }
        public Option<AssertRule>   AssertAfter { get; private set; }
    }

}
