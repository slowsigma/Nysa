using System;

namespace Nysa.Scheduling.Calendars
{

    public class UnavailableDay : CalendarDay
    {
        public DateTime Date        { get; private set; }
        public String   Category    { get; private set; }
        public String   Reason      { get; private set; }

        public UnavailableDay(Int32 weekIndex, Int32 dayIndex, DateTime date, String category, String reason)
            : base(weekIndex, dayIndex)
        {
            this.Date       = date;
            this.Category   = category;
            this.Reason     = reason;
        }
    }

}
