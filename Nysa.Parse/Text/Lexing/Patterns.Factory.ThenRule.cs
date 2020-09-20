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

            public class ThenRule : Rule
            {
                public Rule First { get; private set; }
                public Rule Second { get; private set; }

                public ThenRule(Rule first, Rule second)
                {
                    this.First = first;
                    this.Second = second;
                }
            }

        }

    }

}
