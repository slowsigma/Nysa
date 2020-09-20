using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {

        public class Symbol
        {
            public static readonly Symbol End  = new Symbol("}.{");
            public static readonly Symbol Self = new Symbol(">.<");

            public static implicit operator Symbol(String symbol)
                =>   symbol.Equals("}.{") ? Symbol.End
                   : symbol.Equals(">.<") ? Symbol.Self
                   :                        new Symbol(symbol);

            public static implicit operator Symbol(Factory.ExplicitRuleDefined explicitRuleDefined) => new Symbol(explicitRuleDefined.Symbol);
            public static implicit operator Symbol(Factory.ExplicitRuleClosed explicitRuleClosed) => new Symbol(explicitRuleClosed.Symbol);
            public static implicit operator Symbol(Factory.EitherRuleDefined eitherRuleDefined) => new Symbol(eitherRuleDefined.Symbol);
            public static implicit operator Symbol(Factory.EitherRuleClosed eitherRuleClosed) => new Symbol(eitherRuleClosed.Symbol);
            public static implicit operator Symbol(Factory.RepeatOneOrMoreRule repeatOneOrMoreRule) => new Symbol(repeatOneOrMoreRule.Symbol);
            public static implicit operator Symbol(Factory.RepeatZeroOrMoreRule repeatZeroOrMoreRule) => new Symbol(repeatZeroOrMoreRule.Symbol);

            // instance members
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private String _Value;
            public String Value => this._Value;

            private Symbol(String value)
            {
                this._Value = value;
            }

            public override String ToString() => this._Value;
        }
        
    }

}
