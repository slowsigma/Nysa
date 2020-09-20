using System;
using System.Linq;

namespace Nysa.Text.Lexing.Mini
{

    /// <summary>
    /// AnyRule
    /// </summary>
    public sealed class AnyRule : Rule
    {
        public CasedString Alternatives { get; private set; }
        public CasedString Exceptions   { get; private set; }

        public AnyRule Or(AnyRule others)
            => new AnyRule(this.Alternatives.SetConcat(others.Alternatives),
                              this.Exceptions.SetConcat(others.Exceptions));

        public AnyRule Or(CasedString others)
            => new AnyRule(this.Alternatives.SetConcat(others),
                              this.Exceptions);

        public AnyRule Except(params CasedString[] without)
            => new AnyRule(this.Alternatives,
                           without.Aggregate(this.Exceptions, (a, c) => a.SetConcat(c)));

        internal AnyRule(params CasedString[] alternatives)
            : this(alternatives.Aggregate((a, c) => a.SetConcat(c)), String.Empty)
        { }

        private AnyRule(CasedString alternatives, CasedString exceptions)
        {
            this.Alternatives = alternatives;
            this.Exceptions   = exceptions;
        }
    }

}
