using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

namespace Nysa.Scheduling.Dates
{

    public static class LogicTransforms
    {
        private static Func<DateTime, Boolean> ToAnd(this Func<DateTime, Boolean>[] conditions)
            => d => conditions.All(c => c(d));
        private static Func<DateTime, Boolean> ToAnd(this Func<DateTime, Boolean> first, Func<DateTime, Boolean> second)
            => d => first(d) && second(d);
        private static Func<DateTime, Boolean> ToOr(this Func<DateTime, Boolean>[] conditions)
            => d => conditions.Any(c => c(d));
        private static Func<DateTime, Boolean> ToOr(this Func<DateTime, Boolean> first, Func<DateTime, Boolean> second)
            => d => first(d) || second(d);
        private static Func<DateTime, Boolean> ToNot(this Func<DateTime, Boolean> condition)
            => d => !condition(d);
        private static Func<DateTime, IEnumerable<DateTime>> ToDatesBackward(this Int32 number)
            => d => Enumerable.Range(0, number).Select(i => d.AddDays(i * -1));
        private static Func<DateTime, Boolean> ToDatesCount(this Int32 number, Func<DateTime, IEnumerable<DateTime>> series)
            => d => series(d).Count() == number;
        private static Func<DateTime, IEnumerable<DateTime>> ToDatesFilter(this Func<DateTime, IEnumerable<DateTime>> series, Func<DateTime, Func<DateTime, Boolean>> condition)
            => d => series(d).Where(p => condition(d)(p));
        private static Func<DateTime, IEnumerable<DateTime>> ToDatesForward(this Int32 number)
            => d => Enumerable.Range(0, number).Select(i => d.AddDays(i));

        private static Func<DateTime, Boolean> ToIsDayOfMonth(this Int32 day)
            => d => (Convert.ToInt32((d - new DateTime(d.Year, d.Month, 1)).TotalDays) + 1) == day;
        private static Func<DateTime, Boolean> ToIsDayOfYear(this Int32 day)
            => d => (Convert.ToInt32((d - new DateTime(d.Year, 1, 1)).TotalDays) + 1) == day;

        private static Func<DateTime, Func<DateTime, Boolean>> ToMatchAnd(this Func<DateTime, Func<DateTime, Boolean>> first, Func<DateTime, Func<DateTime, Boolean>> second)
            => d => b => first(d)(b) && second(d)(b);
        private static Func<DateTime, Func<DateTime, Boolean>> ToMatchNot(this Func<DateTime, Func<DateTime, Boolean>> condition)
            => d => b => !condition(d)(b);
        private static Func<DateTime, Func<DateTime, Boolean>> ToMatchOn<T>(this Func<DateTime, T> valueTransform) where T : IEquatable<T>
            => d => b => valueTransform(d).Equals(valueTransform(b));
        private static Func<DateTime, Func<DateTime, Boolean>> ToMatchOr(this Func<DateTime, Func<DateTime, Boolean>> first, Func<DateTime, Func<DateTime, Boolean>> second)
            => d => b => first(d)(b) || second(d)(b);

        private static Dictionary<MatchTypes, Func<DateTime, Int32>> _MatchOnLogic = new Dictionary<MatchTypes, Func<DateTime, int>>()
        {
            { MatchTypes.DayOfMonth,  d => Convert.ToInt32((d - new DateTime(d.Year, d.Month, 1)).TotalDays) + 1 },
            { MatchTypes.DayOfWeek,   d => (Int32)d.DayOfWeek },
            { MatchTypes.DayOfYear,   d => Convert.ToInt32((d - new DateTime(d.Year, 1, 1)).TotalDays) + 1 },
            { MatchTypes.MonthOfYear, d => d.Month }
        };

        private static Func<DateTime, Option<DateSimpleRule>> ToDateRule(this Func<DateTime, Boolean> termLogic, DateSimpleRule rule)
            => d => termLogic(d)
                    ? rule.Some()
                    : Option<DateSimpleRule>.None;

        private static Func<DateTime, Option<DateSimpleRule>> ToDateRules(this Func<DateTime, Option<DateSimpleRule>>[] ruleLogics)
            => d => ruleLogics.Select(l => l(d))
                              .FirstOrNone(r => r is Some<DateSimpleRule>)
                              .Bind(s => s);

        public static Func<DateTime, Boolean> ToLogic(this DateAnd dateAnd)
            => dateAnd.Terms.Select(c => c.ToLogic()).ToArray().ToAnd();

        public static Func<DateTime, Boolean> ToLogic(this DateTerm dateCondition)
            =>   dateCondition is DateAnd           and  ? and.ToLogic()
               : dateCondition is DateNot           not  ? not.ToLogic()
               : dateCondition is DateOr            or   ? or.ToLogic()
               : dateCondition is DatesCount        cnt  ? cnt.ToLogic()
               : dateCondition is IsDayOfMonth      dom  ? dom.ToLogic()
               : dateCondition is IsDayOfWeek       dow  ? dow.ToLogic()
               : dateCondition is IsDayOfYear       doy  ? doy.ToLogic()
               : dateCondition is IsMonthOfYear     moy  ? moy.ToLogic()
               : dateCondition is IsYear            year ? year.ToLogic()
               : dateCondition is RelativeDateIs    rdi  ? rdi.ToLogic()
               :                                           null;

        public static Func<DateTime, Boolean> ToLogic(this DateNot dateNot)
            => dateNot.Term.ToLogic().ToNot();

        public static Func<DateTime, Boolean> ToLogic(this DateOr dateOr)
            => dateOr.Terms.Select(c => c.ToLogic()).ToArray().ToOr();

        public static Func<DateTime, IEnumerable<DateTime>> ToLogic(this DatesBackward datesBackward)
            => datesBackward.Number.ToDatesBackward();

        public static Func<DateTime, IEnumerable<DateTime>> ToLogic(this DateSeries dateSeries)
            =>   dateSeries is DatesBackward backward ? backward.ToLogic()
               : dateSeries is DatesFilter   filter   ? filter.ToLogic()
               : dateSeries is DatesForward  forward  ? forward.ToLogic()
               :                                        null;

        public static Func<DateTime, Boolean> ToLogic(this DatesCount dateSeriesCount)
            => dateSeriesCount.Number.ToDatesCount(dateSeriesCount.Series.ToLogic());

        public static Func<DateTime, IEnumerable<DateTime>> ToLogic(this DatesFilter dateSeriesFilter)
            => dateSeriesFilter.Series.ToLogic().ToDatesFilter(dateSeriesFilter.Condition.ToLogic());

        public static Func<DateTime, IEnumerable<DateTime>> ToLogic(this DatesForward datesForward)
            => datesForward.Number.ToDatesForward();

        public static Func<DateTime, Boolean> ToLogic(this IsDayOfMonth isDayOfMonth)
            => isDayOfMonth.Day.ToIsDayOfMonth();

        public static Func<DateTime, Boolean> ToLogic(this IsDayOfWeek isDayOfWeek)
            => d => d.DayOfWeek == isDayOfWeek.Day;

        public static Func<DateTime, Boolean> ToLogic(this IsDayOfYear isDayOfYear)
            => isDayOfYear.Day.ToIsDayOfYear();

        public static Func<DateTime, Boolean> ToLogic(this IsMonthOfYear isMonthOfYear)
            => d => d.Month == isMonthOfYear.Month;

        public static Func<DateTime, Boolean> ToLogic(this IsYear isYear)
            => d => d.Year == isYear.Year;

        public static Func<DateTime, Boolean> ToLogic(this RelativeDateIs relativeDateIs)
            => relativeDateIs.Condition.ToLogic().Map(l => new Func<DateTime, Boolean>(d => l(d.AddDays(relativeDateIs.Offset))));

        public static Func<DateTime, Func<DateTime, Boolean>> ToLogic(this MatchAnd matchAnd)
            => matchAnd.First.ToLogic().ToMatchAnd(matchAnd.Second.ToLogic());

        public static Func<DateTime, Func<DateTime, Boolean>> ToLogic(this MatchDate matchDate)
            =>   matchDate is MatchAnd and ? and.ToLogic()
               : matchDate is MatchNot not ? not.ToLogic()
               : matchDate is MatchOn  on  ? on.ToLogic()
               : matchDate is MatchOr  or  ? or.ToLogic()
               :                             null;

        public static Func<DateTime, Func<DateTime, Boolean>> ToLogic(this MatchNot matchNot)
            => matchNot.Condition.ToLogic();

        public static Func<DateTime, Func<DateTime, Boolean>> ToLogic(this MatchOn matchOn)
            => _MatchOnLogic[matchOn.Type].ToMatchOn();

        public static Func<DateTime, Func<DateTime, Boolean>> ToLogic(this MatchOr matchOr)
            => matchOr.First.ToLogic().ToMatchOr(matchOr.Second.ToLogic());

        public static Func<DateTime, Option<DateSimpleRule>> ToLogic(this DateSimpleRule dateSimpleRule)
            => ToDateRule(dateSimpleRule.Terms.ToLogic(), dateSimpleRule);

        public static Func<DateTime, Option<DateSimpleRule>> ToLogic(this DateCompoundRule dateCompoundRule)
            => dateCompoundRule.Alternatives
                               .Select(s => s.ToLogic())
                               .ToArray()
                               .Map(a => a.ToDateRules());

        public static Func<DateTime, Option<DateSimpleRule>> ToLogic(this IEnumerable<DateRule> dateRules)
            => dateRules.Select(r => r is DateSimpleRule simple ? simple.ToLogic() : r is DateCompoundRule compound ? compound.ToLogic() : null)
                        .ToArray()
                        .Map(a => a.ToDateRules());
    }

}
