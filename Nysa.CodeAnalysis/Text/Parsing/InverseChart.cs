using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nysa.Text.Parsing;

public sealed record InverseChart : BasicChart
{
    internal InverseChart(Grammar grammar, IReadOnlyList<IReadOnlyList<ChartEntry>?> data)
        : base(grammar, data)
    {
    }

    // public override string ToString()
    // {
    //     var build = new StringBuilder();

    //     foreach (var position in this)
    //     {
    //         build.AppendLine($"Position: [{position.Index}]");

    //         foreach (var entry in position.Where(e => true))
    //         {
    //             build.AppendLine(String.Concat("   ", entry.ToString()));
    //         }
    //     }

    //     return build.ToString();
    // }

}
