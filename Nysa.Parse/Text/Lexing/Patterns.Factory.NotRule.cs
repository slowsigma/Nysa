using System;

namespace Nysa.Text.Lexing
{

    public partial class Patterns
    {

        public partial class Factory
        {

            public class NotRule : Rule
            {
                public Rule What { get; private set; }

                internal NotRule(Rule what)
                {
                    this.What = what;
                }
            }

        }

    }

}
