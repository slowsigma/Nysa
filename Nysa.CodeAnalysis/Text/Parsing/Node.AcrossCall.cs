﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;
using Dorata.Text.Lexing;

namespace Dorata.Text.Parsing
{

    public partial class Node
    {

        private sealed class AcrossCall : BuildCall
        {
            public Option<AcrossItem>   Previous    { get; private set; }
            public Int32                CurrentRule { get; private set; }
            public Int32                LengthLeft  { get; private set; }

            public AcrossCall(BuildStates call, BuildStates @return, FinalChart.Entry entry, FinalChart.Position position, Int32 currentRule, Int32 lengthLeft, AcrossItem? previous = null)
                : base(call, @return, entry, position)
            {
                this.Previous       = previous ?? Option<AcrossItem>.None;
                this.CurrentRule    = currentRule;
                this.LengthLeft     = lengthLeft;
            }
        }

    }

}
