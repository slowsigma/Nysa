using System;

namespace Nysa.Scheduling.Dates
{

    public class IsYear : DateTerm
    {
        public Int32 Year { get; private set; }

        public IsYear(Int32 year)
            => this.Year = year;
    }

}
