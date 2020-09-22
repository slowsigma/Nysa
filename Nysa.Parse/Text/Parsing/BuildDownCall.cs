using System;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    internal sealed class BuildDownCall : BuildCall
    {
        public Option<BuildDown> Above { get; set; }

        public BuildDownCall(BuildStates call, BuildStates @return, ChartEntry entry, ChartPosition position, BuildDown above = null)
            : base(call, @return, entry, position)
        {
            this.Above = above != null
                         ? above.Some()
                         : Option<BuildDown>.None;
        }
    }

}
