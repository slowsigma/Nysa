using System;

using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript;

public interface IParseListener : ICreateChartListener
{
    void CreateChartEnded(Grammar grammar, ParseChart chart);
    void ChartInverted(Grammar grammar, InverseChart chart);
}