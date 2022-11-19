using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public partial class Node
    {

        private class DownItem
        {
            // instance members
            public Option<DownItem>  Previous { get; private set; }
            public ChartEntry  Current  { get; private set; }

            private DownItem(Option<DownItem> previous, ChartEntry current)
            {
                this.Previous = previous;
                this.Current  = current;
            }

            public DownItem(ChartEntry current) : this(Option<DownItem>.None, current) { }
            public DownItem WithNext(ChartEntry current) => new DownItem(this.Some(), current);
            public Boolean Contains(ChartEntry entry) 
                => this.Current.Equals(entry) || this.Previous.Map(p => p.Contains(entry)).Or(false);
        }

    }

}
