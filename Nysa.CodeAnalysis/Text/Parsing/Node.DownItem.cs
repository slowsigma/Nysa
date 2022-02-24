using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using  Nysa.Logics;

using Dorata.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public partial class Node
    {

        private class DownItem
        {
            // instance members
            public Option<DownItem>  Previous { get; private set; }
            public FinalChart.Entry  Current  { get; private set; }

            private DownItem(Option<DownItem> previous, FinalChart.Entry current)
            {
                this.Previous = previous;
                this.Current  = current;
            }

            public DownItem(FinalChart.Entry current) : this(Option<DownItem>.None, current) { }
            public DownItem WithNext(FinalChart.Entry current) => new DownItem(this.Some(), current);
            public Boolean Contains(FinalChart.Entry entry) 
                => this.Current.Equals(entry) || this.Previous.Map(p => p.Contains(entry)).Or(false);
        }

    }

}
