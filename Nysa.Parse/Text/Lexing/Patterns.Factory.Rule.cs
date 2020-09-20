using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing
{

    public partial class Patterns
    {

        public partial class Factory
        {

            public abstract class Rule
            {
                public Rule Or(Rule other) => new OrRule(this, other);
                public Rule Then(Rule next) => new ThenRule(this, next);
            }

        }

    }

}
