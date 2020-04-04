using System;

namespace Nysa.Scheduling.Dates
{

    public class DatesFilter : DateSeries
    {
        public DateSeries   Series      { get; private set; }
        public MatchDate    Condition   { get; private set; }

        public DatesFilter(DateSeries series, MatchDate condition)
        {
            this.Series     = series;
            this.Condition  = condition;
        }
    }

}
