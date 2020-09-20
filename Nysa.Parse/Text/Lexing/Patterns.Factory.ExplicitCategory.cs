using System;

namespace Nysa.Text.Lexing
{

    public partial class Patterns
    {

        public partial class Factory
        {

            public class ExplicitCategory
            {
                private Action<Rule> _Is;

                internal ExplicitCategory(String symbol, Action<String, Rule> defineMethod)
                {
                    this._Is = rule => defineMethod(symbol, rule);
                }

                public void Is(Rule rule) => this._Is(rule);
            }

        }

    }

}
