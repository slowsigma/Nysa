using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using Nysa.Scheduling.Calendars;

namespace Nysa.Scheduling.Dates
{

    public static class Formatting
    {
        public static IEnumerable<CalendarMonth> CalendarYears(this Int32 numberOfYears, Int32 startYear, Func<DateTime, Option<DateSimpleRule>> unavailable)
        {
            foreach (var boy in Enumerable.Range(0, numberOfYears).Select(y => new DateTime(startYear + y, 1, 1)))
            {
                foreach (var bom in Enumerable.Range(0, 12).Select(m => boy.AddMonths(m)))
                {
                    var offset = (Int32)(bom.DayOfWeek);
                    var slots  = offset + Convert.ToInt32((bom.AddMonths(1) - bom).TotalDays);
                    var rmndr  = slots % 7;
                    
                    slots  = slots + ((rmndr > 0) ? (7 - rmndr) : 0);

                    var days = new List<CalendarDay>();

                    foreach (var (dom, index) in Enumerable.Range(0, slots).Select(s => s < offset ? ((DateTime?)null, s) : (bom.AddDays(s - offset), s)))
                    {
                        var calendarDay = dom.HasValue && dom.Value.Month == bom.Month
                                          ? unavailable(dom.Value).Match(c => (CalendarDay)new UnavailableDay(index / 7, index % 7, dom.Value, c.Category, c.Name),
                                                                         () => (CalendarDay)new AvailableDay(index / 7, index % 7, dom.Value))
                                          : (CalendarDay)new OtherMonthDay(index / 7, index % 7);

                        days.Add(calendarDay);
                    }

                    yield return new CalendarMonth(bom, days);
                }
            }
        }

    }

}
