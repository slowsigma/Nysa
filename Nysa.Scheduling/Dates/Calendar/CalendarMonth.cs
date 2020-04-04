using System;
using System.Collections.Generic;

namespace Nysa.Scheduling.Calendars
{

    public class CalendarMonth
    {
        public DateTime                     FirstDate   { get; private set; }
        public IReadOnlyList<CalendarDay>   Days        { get; private set; }

        public CalendarMonth(DateTime firstDate, IReadOnlyList<CalendarDay> days)
        {
            this.FirstDate  = firstDate;
            this.Days       = days;
        }
    }

}
