using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    internal sealed class BuildAcrossToken : BuildAcross
    {
        public Token Token { get; private set; }

        public BuildAcrossToken(ChartEntry entry, ChartPosition nextPosition, Option<BuildAcross> previous, Token token)
            : base(entry, nextPosition, previous)
        {
            this.Token = token;
        }

        public override IEnumerable<NodeOrToken> GetMembers(Grammar grammar)
            => this.Previous.Match(p => p.GetMembers(grammar).Concat(((NodeOrToken)this.Token).Enumerable()),
                                   () => ((NodeOrToken)this.Token).Enumerable());
    }

}
