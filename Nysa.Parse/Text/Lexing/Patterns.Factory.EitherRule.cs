using System;
using System.Linq;

namespace Nysa.Text.Lexing
{

    public partial class Patterns
    {

        public partial class Factory
        {

            public class EitherRule : Rule
            {
                public String Alternatives { get; private set; }
                public String Exceptions { get; private set; }

                public EitherRule Or(EitherRule others)
                    => new EitherRule(String.Concat(this.Alternatives, others.Alternatives),
                                      String.Concat(this.Exceptions, others.Exceptions));
                public EitherRule Or(String others)
                    => new EitherRule(String.Concat(this.Alternatives, others), this.Exceptions);

                public EitherRule Except(params String[] without)
                    => new EitherRule(this.Alternatives, String.Join(String.Empty, without.Select(s => s)));

                internal EitherRule(params String[] alternatives)
                    : this(String.Join(String.Empty, alternatives.Select(s => s)), String.Empty)
                {
                }

                private EitherRule(String alternatives, String exceptions)
                {
                    this.Alternatives = alternatives;
                    this.Exceptions = exceptions;
                }
            }

        }

    }

}
