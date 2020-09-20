using System;

namespace Nysa.Text.Lexing.Mini
{

    public sealed class NotRule : Rule
    {
        public Rule What { get; private set; }

        internal NotRule(Rule what) { this.What = what; }
    }

}
