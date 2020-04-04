using System;

namespace Nysa.Scheduling.Dates
{

    public class DateNot : DateTerm
    {
        public DateTerm Term { get; private set; }

        public DateNot(DateTerm condition)
            => this.Term = condition;
    }

}
