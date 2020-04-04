using System;

namespace Nysa.Scheduling.Dates
{

    public static class Month
    {
        public static IsMonthOfYear Is(Int32 monthOfYear) => new IsMonthOfYear(monthOfYear);

        private static MatchDate SameMonthAndDayType = MatchOn.MonthOfYear.And(MatchOn.DayOfWeek);

        private static DateSeries SameDayAndMonthBackwards  = 31.DaysBackward().Where(SameMonthAndDayType);
        private static DateSeries SameDayAndMonthForward    = 31.DaysForward().Where(SameMonthAndDayType);

        private static DateTerm FirstDayOfType  = SameDayAndMonthBackwards.HasCount(1);
        private static DateTerm SecondDayOfType = SameDayAndMonthBackwards.HasCount(2);
        private static DateTerm ThirdDayOfType  = SameDayAndMonthBackwards.HasCount(3);
        private static DateTerm FourthDayOfType = SameDayAndMonthBackwards.HasCount(4);
        private static DateTerm LastDayOfType   = SameDayAndMonthForward.HasCount(1);

        public static DateTerm First (DayOfWeek dayOfWeek) => new DateAnd(new IsDayOfWeek(dayOfWeek), FirstDayOfType);
        public static DateTerm Second(DayOfWeek dayOfWeek) => new DateAnd(new IsDayOfWeek(dayOfWeek), SecondDayOfType);
        public static DateTerm Third (DayOfWeek dayOfWeek) => new DateAnd(new IsDayOfWeek(dayOfWeek), ThirdDayOfType);
        public static DateTerm Fourth(DayOfWeek dayOfWeek) => new DateAnd(new IsDayOfWeek(dayOfWeek), FourthDayOfType);
        public static DateTerm Last  (DayOfWeek dayOfWeek) => new DateAnd(new IsDayOfWeek(dayOfWeek), LastDayOfType);
    }

}
