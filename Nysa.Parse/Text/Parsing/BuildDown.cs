using System;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    internal class BuildDown
    {
        public Option<BuildDown> Previous { get; private set; }
        public ChartEntry        Current  { get; private set; }

        private BuildDown(Option<BuildDown> previous, ChartEntry current)
        {
            this.Previous = previous;
            this.Current  = current;
        }

        public BuildDown(ChartEntry current) : this(Option<BuildDown>.None, current) { }
        public BuildDown WithNext(ChartEntry current) => new BuildDown(this.Some(), current);
        public Boolean Contains(ChartEntry entry) 
            => this.Current.Equals(entry) || this.Previous.Map(p => p.Contains(entry)).Or(false);
    }

}
