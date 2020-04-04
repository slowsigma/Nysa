using System;

namespace Nysa.Scheduling.Calendars
{

    public class AvailableDay : CalendarDay
    {
        public DateTime Date { get; private set; }

        public AvailableDay(Int32 weekIndex, Int32 dayIndex, DateTime date)
            : base(weekIndex, dayIndex)
        {
            this.Date = date;
        }
    }

}
