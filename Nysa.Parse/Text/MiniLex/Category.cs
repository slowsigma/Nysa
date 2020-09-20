using System;

namespace Nysa.Text.Lexing.Mini
{

    public sealed class Category
    {
        public String   Symbol  { get; private set; }
        public Rule     Rule    { get; private set; }

        internal Category(String symbol, Rule rule)
        {
            this.Symbol = symbol;
            this.Rule   = rule;
        }
    }

}
