using System;

namespace Nysa.Scheduling.Dates
{

    public static class Day
    {
        public static IsDayOfMonth Is(Int32 dayOfMonth) => new IsDayOfMonth(dayOfMonth);

        public static readonly IsDayOfWeek IsSunday = new IsDayOfWeek(DayOfWeek.Sunday);
        public static readonly IsDayOfWeek IsMonday = new IsDayOfWeek(DayOfWeek.Monday);
        public static readonly IsDayOfWeek IsTuesday = new IsDayOfWeek(DayOfWeek.Tuesday);
        public static readonly IsDayOfWeek IsWednesday = new IsDayOfWeek(DayOfWeek.Wednesday);
        public static readonly IsDayOfWeek IsThursday = new IsDayOfWeek(DayOfWeek.Thursday);
        public static readonly IsDayOfWeek IsFriday = new IsDayOfWeek(DayOfWeek.Friday);
        public static readonly IsDayOfWeek IsSaturday = new IsDayOfWeek(DayOfWeek.Saturday);

        public static readonly DateNot IsNotSunday = new DateNot(new IsDayOfWeek(DayOfWeek.Sunday));
        public static readonly DateNot IsNotMonday = new DateNot(new IsDayOfWeek(DayOfWeek.Monday));
        public static readonly DateNot IsNotTuesday = new DateNot(new IsDayOfWeek(DayOfWeek.Tuesday));
        public static readonly DateNot IsNotWednesday = new DateNot(new IsDayOfWeek(DayOfWeek.Wednesday));
        public static readonly DateNot IsNotThursday = new DateNot(new IsDayOfWeek(DayOfWeek.Thursday));
        public static readonly DateNot IsNotFriday = new DateNot(new IsDayOfWeek(DayOfWeek.Friday));
        public static readonly DateNot IsNotSaturday = new DateNot(new IsDayOfWeek(DayOfWeek.Saturday));

        public static DateTerm PriorIs(DateTerm condition) => new RelativeDateIs(-1, condition);
    }

}
