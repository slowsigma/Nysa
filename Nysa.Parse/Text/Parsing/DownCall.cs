using System;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    internal sealed class DownCall : BuildCall
    {
        public Option<BuildDown> Above { get; set; }

        public DownCall(BuildStates call, BuildStates @return, ChartEntry entry, ChartPosition position, BuildDown above = null)
            : base(call, @return, entry, position)
        {
            this.Above = above != null
                         ? above.Some()
                         : Option<BuildDown>.None;
        }
    }

}
