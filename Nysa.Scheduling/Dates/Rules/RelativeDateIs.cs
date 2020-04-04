using System;

namespace Nysa.Scheduling.Dates
{

    public class RelativeDateIs : DateTerm
    {
        public Int32    Offset      { get; private set; }
        public DateTerm Condition   { get; private set; }

        public RelativeDateIs(Int32 offset, DateTerm condition)
        {
            this.Offset     = offset;
            this.Condition  = condition;
        }
    }

}
