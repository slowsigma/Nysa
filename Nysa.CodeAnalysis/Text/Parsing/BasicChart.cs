using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nysa.Text.Parsing;

public abstract record BasicChart(
    Grammar Grammar,
    IReadOnlyList<IReadOnlyList<ChartEntry>?> Data
);