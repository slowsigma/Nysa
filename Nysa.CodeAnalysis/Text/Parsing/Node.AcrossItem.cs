using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;
using Dorata.Text.Lexing;

namespace Dorata.Text.Parsing
{

    public partial class Node
    {

        private abstract class AcrossItem
        {
            // static members
            public static AcrossItem StartWithNode(FinalChart.Entry entry, FinalChart.Position nextPosition, AcrossItem member)
                => new NodeAcrossItem(entry, nextPosition, Option<AcrossItem>.None, member);
            public static AcrossItem StartWithEmpty(FinalChart.Entry entry, FinalChart.Position nextPosition)
                => new EmptyAcrossItem(entry, nextPosition, Option<AcrossItem>.None);
            public static AcrossItem StartWithToken(FinalChart.Entry entry, FinalChart.Position nextPosition, Token token)
                => new TokenAcrossItem(entry, nextPosition, Option<AcrossItem>.None, token);

            private sealed class NodeAcrossItem : AcrossItem
            {
                // instance members
                public AcrossItem   Member  { get; private set; }

                public NodeAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous, AcrossItem member)
                    : base(entry, nextPosition, previous)
                {
                    this.Member = member;
                }

                protected override NodeOrToken? ToMember()
                    => this.Member.AsNode();

                protected override IEnumerable<NodeOrToken?> GetMembers()
                    => this.Previous.Select(p => p.GetMembers().Concat(this.ToMember().Return()), () => this.ToMember().Return());

                public override Node AsNode()
                    => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol, this.GetMembers());
            }

            private sealed class EmptyAcrossItem : AcrossItem
            {
                public EmptyAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous)
                    : base(entry, nextPosition, previous)
                {
                }

                protected override NodeOrToken? ToMember()
                    => null;

                protected override IEnumerable<NodeOrToken?> GetMembers()
                {
                    yield break;
                }

                public override Node AsNode()
                    => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol);
            }

            private sealed class TokenAcrossItem : AcrossItem
            {
                // instance members
                public Token Token { get; private set; }

                public TokenAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous, Token token)
                    : base(entry, nextPosition, previous)
                {
                    this.Token = token;
                }

                protected override NodeOrToken? ToMember()
                    => this.Token;

                protected override IEnumerable<NodeOrToken?> GetMembers()
                    => this.Previous.Select(p => p.GetMembers().Concat(this.ToMember().Return()), () => this.ToMember().Return());

                public override Node AsNode()
                    => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol, this.GetMembers());
            }

            // instance members
            public FinalChart.Entry         Entry           { get; private set; }
            public FinalChart.Position      NextPosition    { get; private set; }
            public Option<AcrossItem>       Previous        { get; private set; }

            protected AcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous)
            {
                this.Entry          = entry;
                this.NextPosition   = nextPosition;
                this.Previous       = previous;
            }

            public AcrossItem NextAcrossNode(FinalChart.Position nextPosition, AcrossItem member)
                => new NodeAcrossItem(this.Entry, nextPosition, this, member);
            public AcrossItem NextAcrossEmpty(FinalChart.Entry empty)
                => new EmptyAcrossItem(this.Entry, this.NextPosition, this);
            public AcrossItem NextAcrossToken(FinalChart.Position nextPosition, Token token)
                => new TokenAcrossItem(this.Entry, nextPosition, this, token);

            protected abstract NodeOrToken? ToMember();

            protected abstract IEnumerable<NodeOrToken?> GetMembers();

            /// <summary>
            /// Returns the complete node represented by this AcrossItem.
            /// </summary>
            /// <returns></returns>
            public abstract Node AsNode();
        }

    }

}
