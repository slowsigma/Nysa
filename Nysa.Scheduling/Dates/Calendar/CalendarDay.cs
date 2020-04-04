using System;

namespace Nysa.Scheduling.Calendars
{

    public abstract class CalendarDay
    {
        public Int32            WeekIndex   { get; private set; }
        public Int32            DayIndex    { get; private set; }
        
        protected CalendarDay(Int32 weekIndex, Int32 dayIndex)
        {
            this.WeekIndex = weekIndex;
            this.DayIndex  = dayIndex;
        }
    }

}
