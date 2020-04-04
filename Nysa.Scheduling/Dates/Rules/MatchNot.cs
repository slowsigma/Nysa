using System;

namespace Nysa.Scheduling.Dates
{

    public class MatchNot : MatchDate
    {
        public MatchDate Condition { get; private set; }

        public MatchNot(MatchDate condition) => this.Condition = condition;
    }

}
