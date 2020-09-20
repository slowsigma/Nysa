using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    internal sealed class BuildAcrossNode : BuildAcross
    {
        // instance members
        public BuildAcross Member { get; private set; }

        public BuildAcrossNode(ChartEntry entry, ChartPosition nextPosition, Option<BuildAcross> previous, BuildAcross member)
            : base(entry, nextPosition, previous)
        {
            this.Member = member;
        }

        public override IEnumerable<NodeOrToken> GetMembers(Grammar grammar)
            => this.Previous.Match(p => p.GetMembers(grammar).Concat(((NodeOrToken)this.Member.AsNode(grammar)).Enumerable()),
                                   () => ((NodeOrToken)this.Member.AsNode(grammar)).Enumerable());
    }

}
