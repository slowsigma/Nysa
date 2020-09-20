using System;

namespace Nysa.Text.Lexing
{

    public partial class Patterns
    {

        public partial class Factory
        {

            public class MaybeRule : Rule
            {
                public Rule What { get; private set; }

                internal MaybeRule(Rule what)
                {
                    this.What = what;
                }
            }

        }

    }

}
