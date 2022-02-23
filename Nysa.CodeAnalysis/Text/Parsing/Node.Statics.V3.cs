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

        /// <summary>
        /// In V3, the goal is to eliminate the building of partial node trees since certain
        /// conditions arise where the trees are tossed out. This happens when parse chart
        /// contains variations arising from ambiguities in the grammar. Here we use the
        /// AcrossItem objects as a stand-in for the tree nodes.
        /// </summary>
        // private static class V3
        // {

        //     public abstract class AcrossItem
        //     {
        //         // static members
        //         public static AcrossItem StartWithNode(FinalChart.Entry entry, FinalChart.Position nextPosition, AcrossItem member)
        //             => new NodeAcrossItem(entry, nextPosition, Option<AcrossItem>.None, member);
        //         public static AcrossItem StartWithEmpty(FinalChart.Entry entry, FinalChart.Position nextPosition)
        //             => new EmptyAcrossItem(entry, nextPosition, Option<AcrossItem>.None);
        //         public static AcrossItem StartWithToken(FinalChart.Entry entry, FinalChart.Position nextPosition, Token token)
        //             => new TokenAcrossItem(entry, nextPosition, Option<AcrossItem>.None, token);

        //         private sealed class NodeAcrossItem : AcrossItem
        //         {
        //             // instance members
        //             public AcrossItem   Member  { get; private set; }

        //             public NodeAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous, AcrossItem member)
        //                 : base(entry, nextPosition, previous)
        //             {
        //                 this.Member = member;
        //             }

        //             protected override NodeOrToken? ToMember()
        //                 => this.Member.AsNode();

        //             protected override IEnumerable<NodeOrToken?> GetMembers()
        //                 => this.Previous.Select(p => p.GetMembers().Concat(this.ToMember().Return()), () => this.ToMember().Return());

        //             public override Node AsNode()
        //                 => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol, this.GetMembers());
        //         }

        //         private sealed class EmptyAcrossItem : AcrossItem
        //         {
        //             // instance members

        //             public EmptyAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous)
        //                 : base(entry, nextPosition, previous)
        //             {
        //             }

        //             protected override NodeOrToken? ToMember()
        //                 => null;

        //             protected override IEnumerable<NodeOrToken?> GetMembers()
        //             {
        //                 yield break;
        //             }

        //             public override Node AsNode()
        //                 => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol);
        //         }

        //         private sealed class TokenAcrossItem : AcrossItem
        //         {
        //             // instance members
        //             public Token Token { get; private set; }

        //             public TokenAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous, Token token)
        //                 : base(entry, nextPosition, previous)
        //             {
        //                 this.Token = token;
        //             }

        //             protected override NodeOrToken? ToMember()
        //                 => this.Token;

        //             protected override IEnumerable<NodeOrToken?> GetMembers()
        //                 => this.Previous.Select(p => p.GetMembers().Concat(this.ToMember().Return()), () => this.ToMember().Return());

        //             public override Node AsNode()
        //                 => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol, this.GetMembers());
        //         }

        //         // instance members
        //         public FinalChart.Entry         Entry           { get; private set; }
        //         public FinalChart.Position      NextPosition    { get; private set; }
        //         public Option<AcrossItem>       Previous        { get; private set; }

        //         protected AcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous)
        //         {
        //             this.Entry          = entry;
        //             this.NextPosition   = nextPosition;
        //             this.Previous       = previous;
        //         }

        //         public AcrossItem NextAcrossNode(FinalChart.Position nextPosition, AcrossItem member)
        //             => new NodeAcrossItem(this.Entry, nextPosition, this, member);
        //         public AcrossItem NextAcrossEmpty(FinalChart.Entry empty)
        //             => new EmptyAcrossItem(this.Entry, this.NextPosition, this);
        //         public AcrossItem NextAcrossToken(FinalChart.Position nextPosition, Token token)
        //             => new TokenAcrossItem(this.Entry, nextPosition, this, token);

        //         protected abstract NodeOrToken? ToMember();

        //         protected abstract IEnumerable<NodeOrToken?> GetMembers();

        //         /// <summary>
        //         /// Returns the complete node represented by this AcrossItem.
        //         /// </summary>
        //         /// <returns></returns>
        //         public abstract Node AsNode();
        //     }

        //     private class DownItem
        //     {
        //         // instance members
        //         public Option<DownItem>  Previous { get; private set; }
        //         public FinalChart.Entry  Current  { get; private set; }

        //         private DownItem(Option<DownItem> previous, FinalChart.Entry current)
        //         {
        //             this.Previous = previous;
        //             this.Current  = current;
        //         }

        //         public DownItem(FinalChart.Entry current) : this(Option<DownItem>.None, current) { }
        //         public DownItem WithNext(FinalChart.Entry current) => new DownItem(this, current);
        //         public Boolean Contains(FinalChart.Entry entry) 
        //             => this.Current.Equals(entry) || this.Previous.Select(p => p.Contains(entry)).Or(false);
        //     }


        //     // Always called with the entry needed: Terminal, Empty, or a Complex
        //     // The return will always be used as Member in a higher NodeAcrossItem 
        //     private static Option<AcrossItem> BuildDown(FinalChart chart, FinalChart.Entry entry, FinalChart.Position position, Option<DownItem> above, Token[] input)
        //     {
        //         if (entry.Rule.IsEmpty)
        //             return (entry.Number == 0)
        //                    ? AcrossItem.StartWithEmpty(entry, position)
        //                    : Option<AcrossItem>.None;

        //         var id = entry.Rule.DefinitionIds[0];

        //         if (chart.Grammar.IsTerminal(id))
        //         {
        //             var token        = input[position.Index];
        //             var nextPosition = chart[position.Index + 1];

        //             return (token.Id == id)
        //                    ? BuildAcross(chart, entry, 1, entry.Number - 1, nextPosition, AcrossItem.StartWithToken(entry, nextPosition, token), input)
        //                    : Option<AcrossItem>.None;
        //         }

        //         above = above.Select(p => p.WithNext(entry), () => new DownItem(entry));

        //         foreach (var match in position.Where(e => e.Rule.Id == id && e.Number <= entry.Number && !above.Value.Contains(e)))
        //         {
        //             var down = BuildDown(chart, match, position, above, input);

        //             if (down.IsSome)
        //             {
        //                 var next  = down.Value.NextPosition;
        //                 var check = BuildAcross(chart, entry, 1, entry.Number - match.Number, next, AcrossItem.StartWithNode(entry, next, down.Value), input);

        //                 if (check.IsSome)
        //                     return check;
        //             }
        //         }

        //         return Option<AcrossItem>.None;
        //     }

        //     // The function of BuildAcross is to create string of one or more AcrossItem objects that represents a valid build
        //     // of a rule from chart entries.
        //     // Note: When BuildAcross calls itself, the entry does not change, the currentRule changes.
        //     private static Option<AcrossItem> BuildAcross(FinalChart chart, FinalChart.Entry entry, Int32 currentRule, Int32 lengthLeft, FinalChart.Position position, Option<AcrossItem> previous, Token[] input)
        //     {
        //         // SUSPECT IF (IT'S POSSIBLE THIS MAY NEVER OCCUR WHEN CALLED FROM BuildAcross OR BuildDown)
        //         if (entry.Rule.IsEmpty)
        //             return entry.Number == 0
        //                    ? previous.Select(p => p.NextAcrossEmpty(entry), () => AcrossItem.StartWithEmpty(entry, position))
        //                    : Option<AcrossItem>.None;

        //         if (currentRule >= entry.Rule.DefinitionIds.Count)
        //             return previous;

        //         var id = entry.Rule.DefinitionIds[currentRule];

        //         if (chart.Grammar.IsTerminal(id))
        //         {
        //             var token        = input[position.Index];
        //             var nextPosition = chart[position.Index + 1];

        //             if (token.Id != id)
        //                 return Option<AcrossItem>.None;

        //             previous = previous.Select(p => p.NextAcrossToken(nextPosition, token),
        //                                         () => AcrossItem.StartWithToken(entry, nextPosition, token));

        //             return BuildAcross(chart, entry, currentRule + 1, lengthLeft - 1, nextPosition, previous, input);
        //         }


        //         foreach (var match in position.Where(e => e.Rule.Id == id && e.Number <= lengthLeft))
        //         {
        //             var down = BuildDown(chart, match, position, Option<DownItem>.None, input);

        //             if (down.IsSome)
        //             {
        //                 var nextPosition = down.Value.NextPosition;
        //                 var check        = BuildAcross(chart, 
        //                                                entry, 
        //                                                currentRule + 1, 
        //                                                lengthLeft - match.Number, 
        //                                                nextPosition,
        //                                                previous.Select(p => p.NextAcrossNode(nextPosition, down.Value),
        //                                                                () => AcrossItem.StartWithNode(entry, nextPosition, down.Value)), 
        //                                                input);

        //                 if (check.IsSome)
        //                     return check;
        //             }
        //         }

        //         return Option<AcrossItem>.None;
        //     }

        //     public static Option<Node> Create(FinalChart chart, Token[] input)
        //     {
        //         var id      = chart.Grammar.StartId;
        //         var symbol  = chart.Grammar.StartSymbol;
        //         var result  = Option<Node>.None;

        //         if (chart.Length > 0)
        //         {
        //             foreach (var entry in chart[0].Where(e => e.Rule.Id == id))
        //             {
        //                 if (entry.Rule.IsEmpty && entry.Number == 0)
        //                     continue;

        //                 var check = BuildAcross(chart, entry, 0, entry.Number, chart[0], Option<AcrossItem>.None, input);

        //                 if (check.IsSome)
        //                     return check.Value.AsNode();

        //                 if (result.IsSome)
        //                     break;
        //             }
        //         }

        //         return result;
        //     }

        // }

    }

}
