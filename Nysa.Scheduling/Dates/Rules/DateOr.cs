using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Scheduling.Dates
{
    
    public class DateOr : DateTerm
    {
        public IReadOnlyList<DateTerm> Terms { get; private set; }

        public DateOr(params DateTerm[] terms)
            => this.Terms = terms;
        public DateOr(IEnumerable<DateTerm> terms)
            => this.Terms = terms.ToArray();
    }

}
