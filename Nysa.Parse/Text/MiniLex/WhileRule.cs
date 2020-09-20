using System;

namespace Nysa.Text.Lexing.Mini
{

    public class WhileRule : Rule
    {
        public Rule What { get; private set; }

        internal WhileRule(Rule what) { this.What = what; }
    }

}
