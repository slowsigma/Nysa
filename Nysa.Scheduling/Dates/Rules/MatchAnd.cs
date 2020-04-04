using System;

namespace Nysa.Scheduling.Dates
{

    public class MatchAnd : MatchDate
    {
        public MatchDate First  { get; private set; }
        public MatchDate Second { get; private set; }

        public MatchAnd(MatchDate first, MatchDate second)
        {
            this.First  = first;
            this.Second = second;
        }
    }

}
