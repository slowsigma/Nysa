using System;

namespace Nysa.Scheduling.Dates
{

    public class DatesBackward : DateSeries
    {
        public Int32 Number { get; private set; }

        public DatesBackward(Int32 number) => this.Number = number;
    }

}
