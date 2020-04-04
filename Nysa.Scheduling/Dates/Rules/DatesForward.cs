using System;

namespace Nysa.Scheduling.Dates
{

    public class DatesForward : DateSeries
    {
        public Int32 Number { get; private set; }

        public DatesForward(Int32 number) => this.Number = number;
    }

}
