using System;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {

        public partial class Factory
        {

            public sealed class ExplicitRule
            {
                private Rule _Rule;

                internal ExplicitRule(Rule rule) { this._Rule = rule; }

                public ExplicitRuleDefined Is(params Symbol[] definition)
                    => new ExplicitRuleDefined(this._Rule.WithDefinition(true, definition));
            }

            public sealed class ExplicitRuleDefined : Rule
            {
                internal ExplicitRuleDefined(Rule rule) : base(rule) { }

                public ExplicitRuleDefined Or(params Symbol[] definition)
                    => (ExplicitRuleDefined)this.WithDefinition(true, definition);
                public ExplicitRuleClosed OrOptional()
                    => new ExplicitRuleClosed((ExplicitRuleDefined)this.WithDefinition(true, new Symbol[] { }));
            }

            public sealed class ExplicitRuleClosed : Rule
            {
                internal ExplicitRuleClosed(ExplicitRuleDefined explicitRuleDefined) : base(explicitRuleDefined) { }
            }

        }

    }
}
