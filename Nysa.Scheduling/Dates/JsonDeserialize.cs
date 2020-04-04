using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Nysa.Json.Deserialize;

namespace Nysa.Scheduling.Dates
{
    
    public static class JsonDeserialize
    {
        private static readonly Func<JsonTextReader, DateAnd>           DateAndReadProperties          = r => new DateAnd(DateTermListRead()(r));
        private static readonly Func<JsonTextReader, DateNot>           DateNotReadProperties          = r => new DateNot(Read.PropertyObject(_DateTermReaders.ReadTypeProperties())(r));
        private static readonly Func<JsonTextReader, DateOr>            DateOrReadProperties           = r => new DateOr(DateTermListRead()(r));
        private static readonly Func<JsonTextReader, DatesBackward>     DatesBackwardReadProperties    = r => new DatesBackward(Read.Int32Property(r));
        private static readonly Func<JsonTextReader, DatesCount>        DatesCountReadProperties       = r => new DatesCount(ReadDateSeriesPropertyObject()(r), Read.Int32Property(r));
        private static readonly Func<JsonTextReader, DatesFilter>       DatesFilterReadProperties      = r => new DatesFilter(ReadDateSeriesPropertyObject()(r), ReadMatchDatePropertyObject()(r));
        private static readonly Func<JsonTextReader, DatesForward>      DatesForwardReadProperties     = r => new DatesForward(Read.Int32Property(r));
        private static readonly Func<JsonTextReader, DateSimpleRule>    DateSimpleRuleReadProperties   = r => new DateSimpleRule(Read.StringProperty(r), Read.StringProperty(r), Read.PropertyObject(_DateTermReaders.ReadTypeProperties())(r));
        private static readonly Func<JsonTextReader, DateCompoundRule>  DateCompoundRuleReadProperties = r => new DateCompoundRule(DateSimpleRuleListRead()(r));
        private static readonly Func<JsonTextReader, IsDayOfMonth>      IsDayOfMonthReadProperties     = r => new IsDayOfMonth(Read.Int32Property(r));
        private static readonly Func<JsonTextReader, IsDayOfWeek>       IsDayOfWeekReadProperties      = r => new IsDayOfWeek((DayOfWeek)Read.Int32Property(r));
        private static readonly Func<JsonTextReader, IsDayOfYear>       IsDayOfYearReadProperties      = r => new IsDayOfYear(Read.Int32Property(r));
        private static readonly Func<JsonTextReader, IsMonthOfYear>     IsMonthOfYearReadProperties    = r => new IsMonthOfYear(Read.Int32Property(r));
        private static readonly Func<JsonTextReader, IsYear>            IsYearReadProperties           = r => new IsYear(Read.Int32Property(r));
        private static readonly Func<JsonTextReader, MatchAnd>          MatchAndReadProperties         = r => new MatchAnd(ReadMatchDatePropertyObject()(r), ReadMatchDatePropertyObject()(r));
        private static readonly Func<JsonTextReader, MatchNot>          MatchNotReadProperties         = r => new MatchNot(ReadMatchDatePropertyObject()(r));
        private static readonly Func<JsonTextReader, MatchOn>           MatchOnReadProperties          = r => new MatchOn((MatchTypes)Read.Int32Property(r));
        private static readonly Func<JsonTextReader, MatchOr>           MatchOrReadProperties          = r => new MatchOr(ReadMatchDatePropertyObject()(r), ReadMatchDatePropertyObject()(r));
        private static readonly Func<JsonTextReader, RelativeDateIs>    RelativeDateIsReadProperties   = r => new RelativeDateIs(Read.Int32Property(r), Read.PropertyObject(_DateTermReaders.ReadTypeProperties())(r));


        private static Func<JsonTextReader, T> ReadTypeProperties<T>(this Dictionary<String, Func<JsonTextReader, T>> types)
            where T : class
            => r =>
            {
                var type        = Read.StringProperty(r);
                var typeReader  = types[type];

                return Read.PropertyObject(typeReader)(r);
            };

        private static readonly Dictionary<String, Func<JsonTextReader, DateSeries>> _DateSeriesReaders = new Dictionary<String, Func<JsonTextReader, DateSeries>>()
        {
            { nameof(DatesBackward), DatesBackwardReadProperties },
            { nameof(DatesFilter),   DatesFilterReadProperties },
            { nameof(DatesForward),  DatesForwardReadProperties }
        };

        private static Func<JsonTextReader, DateSeries> ReadDateSeriesPropertyObject()
            => r => Read.PropertyObject(_DateSeriesReaders.ReadTypeProperties())(r);

        private static readonly Dictionary<String, Func<JsonTextReader, DateTerm>> _DateTermReaders = new Dictionary<String, Func<JsonTextReader, DateTerm>>()
        {
            { nameof(DateAnd),          DateAndReadProperties },
            { nameof(DateNot),          DateNotReadProperties },
            { nameof(DateOr),           DateOrReadProperties },
            { nameof(DatesCount),       DatesCountReadProperties },
            { nameof(IsDayOfMonth),     IsDayOfMonthReadProperties },
            { nameof(IsDayOfWeek),      IsDayOfWeekReadProperties },
            { nameof(IsDayOfYear),      IsDayOfYearReadProperties },
            { nameof(IsMonthOfYear),    IsMonthOfYearReadProperties },
            { nameof(IsYear),           IsYearReadProperties },
            { nameof(RelativeDateIs),   RelativeDateIsReadProperties }
        };

        private static Func<JsonTextReader, IReadOnlyList<DateTerm>> DateTermListRead()
            => r => { Read.PropertyName(r); return Read.Array(_DateTermReaders.ReadTypeProperties())(r); };

        private static readonly Dictionary<String, Func<JsonTextReader, MatchDate>> _MatchDateReaders = new Dictionary<String, Func<JsonTextReader, MatchDate>>()
        {
            { nameof(MatchAnd), MatchAndReadProperties },
            { nameof(MatchNot), MatchNotReadProperties },
            { nameof(MatchOn),  MatchOnReadProperties },
            { nameof(MatchOr),  MatchOrReadProperties }
        };

        private static readonly Dictionary<String, Func<JsonTextReader, DateRule>> _DateRuleReaders = new Dictionary<String, Func<JsonTextReader, DateRule>>()
        {
            { nameof(DateSimpleRule), DateSimpleRuleReadProperties },
            { nameof(DateCompoundRule), DateCompoundRuleReadProperties }
        };

        private static Func<JsonTextReader, IReadOnlyList<DateSimpleRule>> DateSimpleRuleListRead()
            => r => { Read.PropertyName(r); return Read.Array(DateSimpleRuleReadProperties)(r); };

        private static Func<JsonTextReader, MatchDate> ReadMatchDatePropertyObject()
            => r => Read.PropertyObject(_MatchDateReaders.ReadTypeProperties())(r);

        public static DateRule ToDateRule(this String json)
            => json.FromJson(r => Read.Object(_DateRuleReaders.ReadTypeProperties())(r));

        public static IReadOnlyList<DateRule> ToDateRules(this String json)
            => json.FromJson(r => Read.Array(_DateRuleReaders.ReadTypeProperties())(r));

    }

}
