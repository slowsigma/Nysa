using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    internal class BuildAcross
    {
        public ChartEntry           Entry           { get; private set; }
        public ChartPosition        NextPosition    { get; private set; }
        public Option<BuildAcross>  Previous        { get; private set; }

        private Func<BuildAcross, Grammar, IEnumerable<NodeOrToken>> _GetTreeMembers;

        public BuildAcross(ChartEntry entry, ChartPosition nextPosition, Option<BuildAcross> previous, Func<BuildAcross, Grammar, IEnumerable<NodeOrToken>> getTreeMembers)
        {
            this.Entry              = entry;
            this.NextPosition       = nextPosition;
            this.Previous           = previous;
            this._GetTreeMembers    = getTreeMembers;
        }

        public IEnumerable<NodeOrToken> GetMembers(Grammar grammar) => this._GetTreeMembers(this, grammar);

        /// <summary>
        /// Returns the complete node represented by this AcrossItem using GetMembers.
        /// </summary>
        /// <returns></returns>
        public Node AsNode(Grammar grammar)
            => grammar.CreateNode(this.Entry.Rule.Id, this._GetTreeMembers(this, grammar));
    }

}
