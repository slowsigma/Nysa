using System;

namespace Nysa.Scheduling.Dates
{

    public class DatesCount : DateTerm
    {
        public DateSeries   Series { get; private set; }
        public Int32        Number { get; private set; }

        public DatesCount(DateSeries series, Int32 number)
        {
            this.Series = series;
            this.Number = number;
        }
    }

}
