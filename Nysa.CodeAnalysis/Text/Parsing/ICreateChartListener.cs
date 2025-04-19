using System;

namespace Nysa.Text.Parsing;

public interface ICreateChartListener
{
    void ChartChanged(Int32 changeIndex, ParseChart chart);
}
