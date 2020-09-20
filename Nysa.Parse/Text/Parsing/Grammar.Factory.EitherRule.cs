using System;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {

        public partial class Factory
        {

            public sealed class EitherRule
            {
                private Rule _Rule;

                internal EitherRule(Rule rule) { this._Rule = rule; }

                public EitherRuleDefined Or(params Symbol[] definition)
                    => new EitherRuleDefined(this._Rule.WithDefinition(false, definition));
            }

            public sealed class EitherRuleDefined : Rule
            {
                internal EitherRuleDefined(Rule rule) : base(rule) { }

                public EitherRuleDefined Or(params Symbol[] definition)
                    => (EitherRuleDefined)this.WithDefinition(false, definition);

                public EitherRuleClosed OrOptional()
                    => new EitherRuleClosed((EitherRuleDefined)this.WithDefinition(false, new Symbol[] { }));
            }

            public sealed class EitherRuleClosed : Rule
            {
                internal EitherRuleClosed(EitherRuleDefined eitherRuleDefined) : base(eitherRuleDefined) { }
            }

        }

    }

}
