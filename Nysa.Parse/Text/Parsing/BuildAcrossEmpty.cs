using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    internal sealed class BuildAcrossEmpty : BuildAcross
    {
        public BuildAcrossEmpty(ChartEntry entry, ChartPosition nextPosition, Option<BuildAcross> previous)
            : base(entry, nextPosition, previous)
        {
        }

        public override IEnumerable<NodeOrToken> GetMembers(Grammar grammar)
            => new NodeOrToken[] { };
    }

}
