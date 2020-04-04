using System;
using System.IO;
using Xunit;

using Nysa.Logics;
using Nysa.Scheduling.Dates;

namespace Nysa.Scheduling.Testing
{

    public class Serialization
    {
        [Fact]
        public void Save()
        {
            (String name, String category) Holiday(String name) => (name, "Holiday");

            var isWeekend       = ("Weekend", "Weekend").Is(Day.IsSunday.Or(Day.IsSaturday));
            var isNewYears      =     Holiday("New Year's Day").Is(Month.Is(1).And(Day.Is(1), Day.IsNotSunday, Day.IsNotSaturday))
                                  .Or(Holiday("New Year's Day (observed)").IsEither(Month.Is(1).And(Day.Is(2), Day.IsMonday),
                                                                                    Month.Is(12).And(Day.Is(31), Day.IsFriday)));

            var isThirdMonday   = Month.Third(DayOfWeek.Monday);
            var isLastMonday    = Month.Last(DayOfWeek.Monday);

            var isMLK           = Holiday("Martin Luther King Day").Is(Month.Is(1).And(isThirdMonday));
            var isPresidents    = Holiday("President's Day").Is(Month.Is(2).And(isThirdMonday));
            var isMemorial      = Holiday("Memorial Day").Is(Month.Is(5).And(isLastMonday));

            var isIndependence  =     Holiday("Independence Day").Is(Month.Is(7).And(Day.Is(4), Day.IsNotSunday, Day.IsNotSaturday))
                                  .Or(Holiday("Independence Day (observed)").Is(Month.Is(7).AndEither(Day.Is(3).And(Day.IsFriday),
                                                                                                      Day.Is(5).And(Day.IsMonday))));
        
            var isLabor         = Holiday("Labor Day").Is(Month.Is(9).And(Month.First(DayOfWeek.Monday)));
            var isColumbus      = Holiday("Columbus Day").Is(Month.Is(10).And(Month.Second(DayOfWeek.Monday)));

            var isVeterans      =     Holiday("Veteran's Day").Is(Month.Is(11).And(Day.Is(11), Day.IsNotSunday, Day.IsNotSaturday))
                                  .Or(Holiday("Veteran's Day (observed)").Is(Month.Is(11).AndEither(Day.Is(10).And(Day.IsFriday),
                                                                                                    Day.Is(12).And(Day.IsMonday))));

            var isThanksTerm    = Month.Is(11).And(Month.Fourth(DayOfWeek.Thursday));
            var isThanks        = Holiday("Thanksgiving Day").Is(isThanksTerm);
            var isAfterThanks   = Holiday("Day After Thanksgiving").Is(Month.Is(11).And(Day.PriorIs(isThanksTerm)));

            var isChristmas     =     Holiday("Christmas Day").Is(Month.Is(12).And(Day.Is(25).And(Day.IsNotSunday, Day.IsNotSaturday)))
                                  .Or(Holiday("Christmas Day (observed)").Is(Month.Is(12).AndEither(Day.Is(26).And(Day.IsMonday),
                                                                                                    Day.Is(24).And(Day.IsFriday))));

            var rules = new DateRule[] { isWeekend, isNewYears, isMLK, isPresidents, isMemorial, isIndependence, isLabor, isColumbus, isVeterans, isThanks, isAfterThanks, isChristmas };

            File.WriteAllText("HolidayRules.json", rules.ToJson());

            var rulesClone = File.ReadAllText("HolidayRules.json").ToDateRules();

            var currDate = new DateTime(2010, 1, 1);
            var endDate  = new DateTime(2020, 12, 31);

            var logic       = rules.ToLogic();
            var logicClone  = rulesClone.ToLogic();

            var monthNum  = 0;
            var mondayNum = 0;

            Boolean IsWeekDay(DateTime date) => (Int32)date.DayOfWeek >= 1 && (Int32)date.DayOfWeek <= 5;

            while (currDate <= endDate)
            {
                var logicResult         = logic(currDate);
                var logicCloneResult    = logicClone(currDate);

                if (mondayNum != currDate.Month)
                {
                    mondayNum = currDate.Month;
                    mondayNum = 0;
                }

                if (currDate.DayOfWeek == DayOfWeek.Monday)
                    mondayNum++;

                Assert.True(logicResult.Match(s => logicCloneResult.Match(sc => s.Name.Equals(sc.Name, StringComparison.OrdinalIgnoreCase), () => false),
                                              () => logicCloneResult.Match(sc => false, () => true)));

                if (currDate.Month == 1 && currDate.Day == 1 && IsWeekDay(currDate))
                    Assert.True(logicResult.Match(s => s.Name.Equals("New Year's Day"), () => false));
                if (currDate.Month == 1 && mondayNum == 3)
                    Assert.True(logicResult.Match(s => s.Name.Equals("Martin Luther King Day"), () => false));
                if (currDate.Month == 2 && mondayNum == 3)
                    Assert.True(logicResult.Match(s => s.Name.Equals("President's Day"), () => false));

                currDate = currDate.AddDays(1);
            }
        }
    }

}
