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

            public class WhileRule : Rule
            {
                public Rule What { get; private set; }

                internal WhileRule(Rule what)
                {
                    this.What = what;
                }
            }

        }

    }
}
