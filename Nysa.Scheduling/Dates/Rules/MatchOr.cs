using System;

namespace Nysa.Scheduling.Dates
{

    public class MatchOr : MatchDate
    {
        public MatchDate First  { get; private set; }
        public MatchDate Second { get; private set; }

        public MatchOr(MatchDate first, MatchDate second)
        {
            this.First  = first;
            this.Second = second;
        }
    }

}
