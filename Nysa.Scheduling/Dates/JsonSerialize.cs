using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Nysa.Json.Serialize;
using Nysa.Logics;

namespace Nysa.Scheduling.Dates
{

    public static class JsonSerialize
    {
        private static Unit WriteDateAnd(JsonTextWriter writer, DateAnd dateAnd)
            => nameof(DateAnd.Terms).WriteProperty(dateAnd.Terms.WriteArray((w, t) => t.WriteObject(WriteDateTerm)(w)))(writer);

        private static Unit WriteDateNot(JsonTextWriter writer, DateNot dateNot)
            => nameof(DateNot.Term).WriteObjectProperty(dateNot.Term, WriteDateTerm)(writer);

        private static Unit WriteDateOr(JsonTextWriter writer, DateOr dateOr)
            => nameof(DateOr.Terms).WriteProperty(dateOr.Terms.WriteArray((w, t) => t.WriteObject(WriteDateTerm)(w)))(writer);

        private static Unit WriteDatesBackward(JsonTextWriter writer, DatesBackward datesBackward)
            => nameof(DatesBackward.Number).WriteProperty(datesBackward.Number.WriteValue())(writer);
        
        private static Unit WriteDatesCount(JsonTextWriter writer, DatesCount datesCount)
            => nameof(DatesCount.Series).WriteObjectProperty(datesCount.Series, WriteDateSeries).Then(
               nameof(DatesCount.Number).WriteProperty(datesCount.Number.WriteValue()))(writer);

        private static Unit WriteDatesFilter(JsonTextWriter writer, DatesFilter datesFilter)
            => nameof(DatesFilter.Series).WriteObjectProperty(datesFilter.Series, WriteDateSeries).Then(
               nameof(DatesFilter.Condition).WriteObjectProperty(datesFilter.Condition, WriteMatchDate))(writer);

        private static Unit WriteDatesForward(JsonTextWriter writer, DatesForward datesForward)
            => nameof(DatesForward.Number).WriteProperty(datesForward.Number.WriteValue())(writer);
        
        private static Unit WriteDateRule(JsonTextWriter writer, DateRule dateRule)
            =>   dateRule is DateSimpleRule   simple   ? nameof(Type).WriteValueProperty(nameof(DateSimpleRule)  ).Then(nameof(DateRule).WriteObjectProperty(simple,   WriteDateSimpleRule))(writer)
               : dateRule is DateCompoundRule compound ? nameof(Type).WriteValueProperty(nameof(DateCompoundRule)).Then(nameof(DateRule).WriteObjectProperty(compound, WriteDateCompoundRule))(writer)
               :                                         throw new InvalidCastException($"The method, {nameof(WriteDateSeries)}, does not accept objects of type: {dateRule.GetType().Name}");

        private static Unit WriteDateSeries(JsonTextWriter writer, DateSeries dateSeries)
            =>   dateSeries is DatesBackward backward ? nameof(Type).WriteValueProperty(nameof(DatesBackward)).Then(nameof(DateSeries).WriteObjectProperty(backward, WriteDatesBackward))(writer)
               : dateSeries is DatesFilter   filter   ? nameof(Type).WriteValueProperty(nameof(DatesFilter)  ).Then(nameof(DateSeries).WriteObjectProperty(filter, WriteDatesFilter))(writer)
               : dateSeries is DatesForward  forward  ? nameof(Type).WriteValueProperty(nameof(DatesForward) ).Then(nameof(DateSeries).WriteObjectProperty(forward, WriteDatesForward))(writer)
               :                                        throw new InvalidCastException($"The method, {nameof(WriteDateSeries)}, does not accept objects of type: {dateSeries.GetType().Name}");

        private static Unit WriteDateTerm(JsonTextWriter writer, DateTerm dateTerm)
            =>   dateTerm is DateAnd        and   ? nameof(Type).WriteValueProperty(nameof(DateAnd       )).Then(nameof(DateTerm).WriteObjectProperty(and, WriteDateAnd))(writer)
               : dateTerm is DateNot        not   ? nameof(Type).WriteValueProperty(nameof(DateNot       )).Then(nameof(DateTerm).WriteObjectProperty(not, WriteDateNot))(writer)
               : dateTerm is DateOr         or    ? nameof(Type).WriteValueProperty(nameof(DateOr        )).Then(nameof(DateTerm).WriteObjectProperty(or, WriteDateOr))(writer)
               : dateTerm is DatesCount     count ? nameof(Type).WriteValueProperty(nameof(DatesCount    )).Then(nameof(DateTerm).WriteObjectProperty(count, WriteDatesCount))(writer)
               : dateTerm is IsDayOfMonth   dom   ? nameof(Type).WriteValueProperty(nameof(IsDayOfMonth  )).Then(nameof(DateTerm).WriteObjectProperty(dom, WriteIsDayOfMonth))(writer)
               : dateTerm is IsDayOfWeek    dow   ? nameof(Type).WriteValueProperty(nameof(IsDayOfWeek   )).Then(nameof(DateTerm).WriteObjectProperty(dow, WriteIsDayOfWeek))(writer)
               : dateTerm is IsDayOfYear    doy   ? nameof(Type).WriteValueProperty(nameof(IsDayOfYear   )).Then(nameof(DateTerm).WriteObjectProperty(doy, WriteIsDayOfYear))(writer)
               : dateTerm is IsMonthOfYear  moy   ? nameof(Type).WriteValueProperty(nameof(IsMonthOfYear )).Then(nameof(DateTerm).WriteObjectProperty(moy, WriteIsMonthOfYear))(writer)
               : dateTerm is IsYear         year  ? nameof(Type).WriteValueProperty(nameof(IsYear        )).Then(nameof(DateTerm).WriteObjectProperty(year, WriteIsYear))(writer)
               : dateTerm is RelativeDateIs rdi   ? nameof(Type).WriteValueProperty(nameof(RelativeDateIs)).Then(nameof(DateTerm).WriteObjectProperty(rdi, WriteRelativeDateIs))(writer)
               :                                   throw new InvalidCastException($"The method, {nameof(WriteDateTerm)}, does not accept objects of type: {dateTerm.GetType().Name}");

        private static Unit WriteDateSimpleRule(JsonTextWriter writer, DateSimpleRule dateSimpleRule)
            => nameof(DateSimpleRule.Name).WriteProperty(dateSimpleRule.Name.WriteValue()).Then(
               nameof(DateSimpleRule.Category).WriteProperty(dateSimpleRule.Category.WriteValue())).Then(
               nameof(DateSimpleRule.Terms).WriteObjectProperty(dateSimpleRule.Terms, WriteDateTerm))(writer);

        private static Unit WriteDateCompoundRule(JsonTextWriter writer, DateCompoundRule dateCompoundRule)
            => nameof(dateCompoundRule.Alternatives).WriteProperty(dateCompoundRule.Alternatives.WriteArray((w, t) => t.WriteObject(WriteDateSimpleRule)(w)))(writer);

        private static Unit WriteIsDayOfMonth(JsonTextWriter writer, IsDayOfMonth isDayOfMonth)
            => nameof(IsDayOfMonth.Day).WriteProperty(isDayOfMonth.Day.WriteValue())(writer);

        private static Unit WriteIsDayOfWeek(JsonTextWriter writer, IsDayOfWeek isDayOfWeek)
            => nameof(IsDayOfWeek.Day).WriteProperty(isDayOfWeek.Day.WriteValue())(writer);
        
        private static Unit WriteIsDayOfYear(JsonTextWriter writer, IsDayOfYear isDayOfYear)
            => nameof(IsDayOfYear.Day).WriteProperty(isDayOfYear.Day.WriteValue())(writer);

        private static Unit WriteIsMonthOfYear(JsonTextWriter writer, IsMonthOfYear isMonthOfYear)
            => nameof(IsMonthOfYear.Month).WriteProperty(isMonthOfYear.Month.WriteValue())(writer);
        
        private static Unit WriteIsYear(JsonTextWriter writer, IsYear isYear)
            => nameof(IsYear.Year).WriteProperty(isYear.Year.WriteValue())(writer);

        private static Unit WriteMatchOn(JsonTextWriter writer, MatchOn matchOn)
            => nameof(MatchOn.Type).WriteProperty(((Int32)matchOn.Type).WriteValue())(writer);

        private static Unit WriteMatchAnd(JsonTextWriter writer, MatchAnd matchAnd)
            => nameof(MatchAnd.First).WriteObjectProperty(matchAnd.First, WriteMatchDate).Then(
               nameof(MatchAnd.Second).WriteObjectProperty(matchAnd.Second, WriteMatchDate))(writer);

        private static Unit WriteMatchOr(JsonTextWriter writer, MatchOr matchOr)
            => nameof(MatchOr.First).WriteObjectProperty(matchOr.First, WriteMatchDate).Then(
               nameof(MatchOr.Second).WriteObjectProperty(matchOr.Second, WriteMatchDate))(writer);

        private static Unit WriteMatchNot(JsonTextWriter writer, MatchNot matchNot)
            => nameof(MatchNot.Condition).WriteObjectProperty(matchNot.Condition, WriteMatchDate)(writer);

        private static Unit WriteMatchDate(JsonTextWriter writer, MatchDate matchDate)
            =>   matchDate is MatchAnd and ? nameof(Type).WriteValueProperty(nameof(MatchAnd)).Then(nameof(MatchDate).WriteObjectProperty(and, WriteMatchAnd))(writer)
               : matchDate is MatchNot not ? nameof(Type).WriteValueProperty(nameof(MatchNot)).Then(nameof(MatchDate).WriteObjectProperty(not, WriteMatchNot))(writer)
               : matchDate is MatchOn  on  ? nameof(Type).WriteValueProperty(nameof(MatchOn )).Then(nameof(MatchDate).WriteObjectProperty(on, WriteMatchOn))(writer)
               : matchDate is MatchOr  or  ? nameof(Type).WriteValueProperty(nameof(MatchOr )).Then(nameof(MatchDate).WriteObjectProperty(or, WriteMatchOr))(writer)
               :                             throw new InvalidCastException($"The method, {nameof(WriteMatchDate)}, does not accept objects of type: {matchDate.GetType().Name}");

        private static Unit WriteRelativeDateIs(JsonTextWriter writer, RelativeDateIs relativeDateIs)
            => nameof(RelativeDateIs.Offset).WriteProperty(relativeDateIs.Offset.WriteValue()).Then(
               nameof(RelativeDateIs.Condition).WriteObjectProperty(relativeDateIs.Condition, WriteDateTerm))(writer);

        public static String ToJson(this DateRule dateRule)
            => dateRule.ToJson(w => dateRule.WriteObject(WriteDateRule)(w));

        public static String ToJson(this IEnumerable<DateRule> dateRules)
            => dateRules.ToJson(w => dateRules.WriteArray((writer, rule) => rule.WriteObject(WriteDateRule)(writer))(w));
    }

}
