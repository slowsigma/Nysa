using System;

namespace Nysa.Scheduling.Dates
{

    public class IsDayOfWeek : DateTerm
    {
        public DayOfWeek Day { get; private set; }

        public IsDayOfWeek(DayOfWeek day)
            => this.Day = day;
    }

}
