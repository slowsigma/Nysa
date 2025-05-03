using System;

namespace Nysa.Text.Parsing;

public interface ICreateChartListener
{
    [Obsolete("This overload is obsolete and no longer called by the parse algorithm. Use the other 'ChartChanged' overload instead.")]
    void ChartChanged(Int32 changeIndex, ParseChart chart);
    void ChartChanged(Int32 currentChartIndex, Int32 currentEntryIndex, ParseChart chart, Int32 changeChartIndex);
}
