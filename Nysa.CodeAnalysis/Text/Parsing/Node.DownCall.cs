using System;
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

        private sealed class DownCall : BuildCall
        {
            public Option<DownItem> Above { get; set; }

            public DownCall(BuildStates call, BuildStates @return, FinalChart.Entry entry, FinalChart.Position position, DownItem? above = null)
                : base(call, @return, entry, position)
            {
                this.Above = above ?? Option<DownItem>.None;
            }
        }

    }

}
