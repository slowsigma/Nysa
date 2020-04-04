using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Scheduling.Dates
{
    
    public class DateAnd : DateTerm
    {
        public IReadOnlyList<DateTerm> Terms { get; private set; }

        public DateAnd(params DateTerm[] terms)
            => this.Terms = terms;
        public DateAnd(IEnumerable<DateTerm> terms)
            => this.Terms = terms.ToArray();
    }

}
