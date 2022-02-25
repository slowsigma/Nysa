﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

using Dorata.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public partial class Node
    {

        private abstract class BuildCall
        {
            public BuildStates          CallState   { get; private set; }
            public BuildStates          ReturnState { get; private set; }
            public FinalChart.Entry     Entry       { get; private set; }
            public FinalChart.Position  Position    { get; private set; }

            public Identifier           SearchId    { get; set; }
            public FinalChart.Entry     Match       { get; set; }
            public Int32                MatchIndex  { get; set; }
            public Option<AcrossItem>   Result      { get; set; }

            protected BuildCall(BuildStates call, BuildStates @return, FinalChart.Entry entry, FinalChart.Position position)
            {
                this.CallState   = call;
                this.ReturnState = @return;
                this.Entry       = entry;
                this.Position    = position;
                this.MatchIndex  = -1;
                this.Result      = Option<AcrossItem>.None;
            }

            public void SetCallState(BuildStates newCallState) { this.CallState = newCallState; }
        }

    }

}