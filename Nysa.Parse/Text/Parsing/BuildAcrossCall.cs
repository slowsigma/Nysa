using System;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    internal sealed class BuildAcrossCall : BuildCall
    {
        public Option<BuildAcross>  Previous    { get; private set; }
        public Int32                CurrentRule { get; private set; }
        public Int32                LengthLeft  { get; private set; }

        public BuildAcrossCall(BuildStates call, BuildStates @return, ChartEntry entry, ChartPosition position, Int32 currentRule, Int32 lengthLeft, BuildAcross previous = null)
            : base(call, @return, entry, position)
        {
            this.Previous       = previous != null
                                  ? previous.Some()
                                  : Option<BuildAcross>.None;
            this.CurrentRule    = currentRule;
            this.LengthLeft     = lengthLeft;
        }
    }

}
