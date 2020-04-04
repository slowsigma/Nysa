using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Scheduling.Dates
{

    public class DateCompoundRule : DateRule
    {
        public IReadOnlyList<DateSimpleRule> Alternatives { get; private set; }

        public DateCompoundRule(params DateSimpleRule[] alternatives)
            => this.Alternatives = alternatives;

        public DateCompoundRule(IEnumerable<DateSimpleRule> alternatives)
            => this.Alternatives = alternatives.ToArray();
    }

}
