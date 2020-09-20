using System;

namespace Nysa.Text.Lexing
{

    public partial class Patterns
    {

        public partial class Factory
        {

            public class OrRule : Rule
            {
                public Rule First { get; private set; }
                public Rule Second { get; private set; }

                public OrRule(Rule first, Rule second)
                {
                    this.First = first;
                    this.Second = second;
                }
            }

        }

    }

}
