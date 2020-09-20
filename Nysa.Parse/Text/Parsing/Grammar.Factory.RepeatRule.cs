using System;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {

        public partial class Factory
        {

            public sealed class RepeatRule
            {
                private Rule _Rule;

                internal RepeatRule(Rule rule) { this._Rule = rule; }

                public RepeatZeroOrMoreRule ZeroOrMore()
                    => new RepeatZeroOrMoreRule(this._Rule.WithDefinition(true, new Symbol[] { }));
                public RepeatOneOrMoreRule OneOrMore()
                    => new RepeatOneOrMoreRule(this._Rule);
            }

            public sealed class RepeatZeroOrMoreRule : Rule
            {
                internal RepeatZeroOrMoreRule(Rule rule) : base(rule) { }
            }

            public sealed class RepeatOneOrMoreRule : Rule
            {
                internal RepeatOneOrMoreRule(Rule rule) : base(rule) { }
            }


        }

    }

}
