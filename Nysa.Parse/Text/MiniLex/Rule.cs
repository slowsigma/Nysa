using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing.Mini
{

    public abstract class Rule
    {
        public Rule Or(Rule other) => new OrRule(this, other);
        public Rule Then(Rule next) => new ThenRule(this, next);
        public Category ToCategory(String symbol) => new Category(symbol, this);
    }

}
