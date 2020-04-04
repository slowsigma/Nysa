using System;
using System.Linq;

namespace Nysa.Scheduling.Dates
{

    public static class RuleExtensions
    {
        public static DateSimpleRule Is(this (String name, String category) descriptor, DateTerm dateCondition)
            => new DateSimpleRule(descriptor.name, descriptor.category, dateCondition);

        public static DateSimpleRule IsEither(this (String name, String category) descriptor, params DateTerm[] alternatives)
            => new DateSimpleRule(descriptor.name, descriptor.category, new DateOr(alternatives));

        public static DateCompoundRule Or(this DateSimpleRule simpleRule, DateSimpleRule alternative)
            => new DateCompoundRule(simpleRule, alternative);

        public static DatesBackward DaysBackward(this Int32 number) => new DatesBackward(number);
        public static DatesForward DaysForward(this Int32 number) => new DatesForward(number);
        public static DateSeries Where(this DateSeries basis, MatchDate match) => new DatesFilter(basis, match);
        public static DatesCount HasCount(this DateSeries dateSeries, Int32 number) => new DatesCount(dateSeries, number);

        public static DateTerm And(this DateTerm first, params DateTerm[] others) => new DateAnd(others.Prepend(first));
        public static DateTerm Or(this DateTerm first, params DateTerm[] others) => new DateOr(others.Prepend(first));
        public static DateTerm AndEither(this DateTerm first, params DateTerm[] alternatives) => new DateAnd(first, new DateOr(alternatives));

        public static MatchDate And(this MatchDate first, MatchDate second) => new MatchAnd(first, second);
        public static MatchDate Or(this MatchDate first, MatchDate second) => new MatchOr(first, second);
    }

}
