﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing.Building;

internal sealed class AcrossCall : BuildCall
{
    public Option<AcrossItem>   Previous    { get; private set; }
    public Int32                CurrentRule { get; private set; }
    public Int32                LengthLeft  { get; private set; }

    public AcrossCall(BuildStates call, BuildStates @return, ChartEntry entry, Int32 position, Int32 currentRule, Int32 lengthLeft, AcrossItem? previous = null)
        : base(call, @return, entry, position)
    {
        this.Previous = previous == null ? Option<AcrossItem>.None : previous.Some();
        this.CurrentRule = currentRule;
        this.LengthLeft = lengthLeft;
    }
}
