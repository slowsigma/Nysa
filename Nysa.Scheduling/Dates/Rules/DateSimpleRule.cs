using System;

namespace Nysa.Scheduling.Dates
{
    
    public class DateSimpleRule : DateRule
    {
        public String   Name        { get; private set; }
        public String   Category    { get; private set; }
        public DateTerm Terms       { get; private set; }

        public DateSimpleRule(String name, String category, DateTerm terms)
        {
            this.Name       = name;
            this.Category   = category;
            this.Terms      = terms;
        }
    }

}
