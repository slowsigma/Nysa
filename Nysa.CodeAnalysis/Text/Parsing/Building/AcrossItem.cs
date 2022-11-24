using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing.Building;

internal abstract class AcrossItem
{
    // static members
    public static AcrossItem StartWithNode(ChartEntry entry, ChartPosition nextPosition, AcrossItem member)
        => new NodeAcrossItem(entry, nextPosition, Option<AcrossItem>.None, member);
    public static AcrossItem StartWithEmpty(ChartEntry entry, ChartPosition nextPosition)
        => new EmptyAcrossItem(entry, nextPosition, Option<AcrossItem>.None);
    public static AcrossItem StartWithToken(ChartEntry entry, ChartPosition nextPosition, Token token)
        => new TokenAcrossItem(entry, nextPosition, Option<AcrossItem>.None, token);

    private sealed class NodeAcrossItem : AcrossItem
    {
        // instance members
        public AcrossItem   Member  { get; private set; }

        public NodeAcrossItem(ChartEntry entry, ChartPosition nextPosition, Option<AcrossItem> previous, AcrossItem member)
            : base(entry, nextPosition, previous)
        {
            this.Member = member;
        }

        protected override NodeOrToken? ToMember()
            => this.Member.AsNode();

        protected override IEnumerable<NodeOrToken?> GetMembers()
            => this.Previous.Match(p  => p.GetMembers().Concat(Return.Enumerable(this.ToMember())),
                                    () => Return.Enumerable(this.ToMember()));

        public override Node AsNode()
            => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol, this.GetMembers());
    }

    private sealed class EmptyAcrossItem : AcrossItem
    {
        public EmptyAcrossItem(ChartEntry entry, ChartPosition nextPosition, Option<AcrossItem> previous)
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

        public TokenAcrossItem(ChartEntry entry, ChartPosition nextPosition, Option<AcrossItem> previous, Token token)
            : base(entry, nextPosition, previous)
        {
            this.Token = token;
        }

        protected override NodeOrToken? ToMember()
            => this.Token;

        protected override IEnumerable<NodeOrToken?> GetMembers()
            => this.Previous.Match(p  => p.GetMembers().Concat(Return.Enumerable(this.ToMember())),
                                    () => Return.Enumerable(this.ToMember()));

        public override Node AsNode()
            => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol, this.GetMembers());
    }

    // instance members
    public ChartEntry           Entry           { get; private set; }
    public ChartPosition        NextPosition    { get; private set; }
    public Option<AcrossItem>   Previous        { get; private set; }

    protected AcrossItem(ChartEntry entry, ChartPosition nextPosition, Option<AcrossItem> previous)
    {
        this.Entry          = entry;
        this.NextPosition   = nextPosition;
        this.Previous       = previous;
    }

    public AcrossItem NextAcrossNode(ChartPosition nextPosition, AcrossItem member)
        => new NodeAcrossItem(this.Entry, nextPosition, this.Some(), member);
    public AcrossItem NextAcrossEmpty(ChartEntry empty)
        => new EmptyAcrossItem(this.Entry, this.NextPosition, this.Some());
    public AcrossItem NextAcrossToken(ChartPosition nextPosition, Token token)
        => new TokenAcrossItem(this.Entry, nextPosition, this.Some(), token);

    protected abstract NodeOrToken? ToMember();

    protected abstract IEnumerable<NodeOrToken?> GetMembers();

    /// <summary>
    /// Returns the complete node represented by this AcrossItem.
    /// </summary>
    /// <returns></returns>
    public abstract Node AsNode();
}
