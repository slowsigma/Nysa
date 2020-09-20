using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Text.Lexing.Definition
{

    public class TakeRule : LiteralRule
    {
        public CasedString Value { get; private set; }

        internal TakeRule(CasedString value) { this.Value = value; }
    }

}
