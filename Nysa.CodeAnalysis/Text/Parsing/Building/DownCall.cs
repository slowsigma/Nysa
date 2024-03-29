﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing.Building;

internal sealed class DownCall : BuildCall
{
    public Option<DownItem> Above { get; set; }

    public DownCall(BuildStates call, BuildStates @return, ChartEntry entry, Int32 position, DownItem? above = null)
        : base(call, @return, entry, position)
    {
        this.Above = above == null ? Option<DownItem>.None : above.Some();
    }
}
