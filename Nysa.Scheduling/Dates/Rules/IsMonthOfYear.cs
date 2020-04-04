using System;

namespace Nysa.Scheduling.Dates
{

    public class IsMonthOfYear : DateTerm
    {
        public Int32 Month { get; private set; }

        public IsMonthOfYear(Int32 month)
            => this.Month = month;
    }

}
