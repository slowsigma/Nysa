using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Text.Lexing.Mini
{

    public static class RulesExtensions
    {

        public static Category ToCategory(this String symbol, Rule forRule)
            => new Category(symbol, forRule);

    }

}
