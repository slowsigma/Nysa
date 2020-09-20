using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    internal abstract class BuildAcross
    {
        // static members
        public static BuildAcross StartWithNode(ChartEntry entry, ChartPosition nextPosition, BuildAcross member)
            => new BuildAcrossNode(entry, nextPosition, Option<BuildAcross>.None, member);
        public static BuildAcross StartWithEmpty(ChartEntry entry, ChartPosition nextPosition)
            => new BuildAcrossEmpty(entry, nextPosition, Option<BuildAcross>.None);
        public static BuildAcross StartWithToken(ChartEntry entry, ChartPosition nextPosition, Token token)
            => new BuildAcrossToken(entry, nextPosition, Option<BuildAcross>.None, token);


        // instance members
        public ChartEntry           Entry           { get; private set; }
        public ChartPosition        NextPosition    { get; private set; }
        public Option<BuildAcross>  Previous        { get; private set; }

        protected BuildAcross(ChartEntry entry, ChartPosition nextPosition, Option<BuildAcross> previous)
        {
            this.Entry          = entry;
            this.NextPosition   = nextPosition;
            this.Previous       = previous;
        }

        public BuildAcross NextAcrossNode(ChartPosition nextPosition, BuildAcross member)
            => new BuildAcrossNode(this.Entry, nextPosition, this.Some(), member);
        public BuildAcross NextAcrossEmpty(ChartEntry empty)
            => new BuildAcrossEmpty(this.Entry, this.NextPosition, this.Some());
        public BuildAcross NextAcrossToken(ChartPosition nextPosition, Token token)
            => new BuildAcrossToken(this.Entry, nextPosition, this.Some(), token);

        public abstract IEnumerable<NodeOrToken> GetMembers(Grammar grammar);

        /// <summary>
        /// Returns the complete node represented by this AcrossItem using GetMembers.
        /// </summary>
        /// <returns></returns>
        public Node AsNode(Grammar grammar)
            => grammar.CreateNode(this.Entry.Rule.Id, this.GetMembers(grammar));
    }

}
