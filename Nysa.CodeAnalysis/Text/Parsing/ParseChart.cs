using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nysa.Text.Parsing;

public sealed record ParseChart(
    Grammar Grammar,
    IReadOnlyList<IReadOnlyList<ChartEntry>?> Data
) : BasicChart(Grammar, Data);
