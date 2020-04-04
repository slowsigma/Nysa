using System;

namespace Nysa.Scheduling.Dates
{

    public class IsDayOfYear : DateTerm
    {
        public Int32 Day { get; private set; }

        public IsDayOfYear(Int32 day)
            => this.Day = day;
    }

}
