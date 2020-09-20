using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nysa.Text.Lexing.Definition
{

    public class AnyRule : LiteralRule
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
