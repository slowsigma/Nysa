using System;

namespace Nysa.Scheduling.Dates
{

    public class IsDayOfMonth : DateTerm
    {
        public Int32 Day { get; private set; }

        public IsDayOfMonth(Int32 day)
            => this.Day = day;
    }

}
