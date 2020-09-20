using System;

namespace Nysa.Text.Lexing
{

    public partial class Patterns
    {

        public partial class Factory
        {

            public class StackRule : Rule
            {
                public Rule Push { get; private set; }
                public Rule Pop { get; private set; }

                internal StackRule(Rule push, Rule pop)
                {
                    this.Push = push;
                    this.Pop = pop;
                }
            }

        }

    }

}
