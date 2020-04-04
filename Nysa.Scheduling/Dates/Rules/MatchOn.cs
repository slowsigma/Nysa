using System;

namespace Nysa.Scheduling.Dates
{

    public class MatchOn : MatchDate
    {
        public static readonly MatchOn DayOfMonth   = new MatchOn(MatchTypes.DayOfMonth);
        public static readonly MatchOn DayOfWeek    = new MatchOn(MatchTypes.DayOfWeek);
        public static readonly MatchOn DayOfYear    = new MatchOn(MatchTypes.DayOfYear);
        public static readonly MatchOn MonthOfYear  = new MatchOn(MatchTypes.MonthOfYear);

        public MatchTypes Type { get; private set; }

        public MatchOn(MatchTypes type) => this.Type = type;
    }
}
