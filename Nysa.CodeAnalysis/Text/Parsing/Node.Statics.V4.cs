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

//         private static class V4
//         {
// #if DEBUG
//             public static Int32 _DownCallSaves = 0;
// #endif

//             public abstract class AcrossItem
//             {
//                 // static members
//                 public static AcrossItem StartWithNode(FinalChart.Entry entry, FinalChart.Position nextPosition, AcrossItem member)
//                     => new NodeAcrossItem(entry, nextPosition, Option<AcrossItem>.None, member);
//                 public static AcrossItem StartWithEmpty(FinalChart.Entry entry, FinalChart.Position nextPosition)
//                     => new EmptyAcrossItem(entry, nextPosition, Option<AcrossItem>.None);
//                 public static AcrossItem StartWithToken(FinalChart.Entry entry, FinalChart.Position nextPosition, Token token)
//                     => new TokenAcrossItem(entry, nextPosition, Option<AcrossItem>.None, token);

//                 private sealed class NodeAcrossItem : AcrossItem
//                 {
//                     // instance members
//                     public AcrossItem   Member  { get; private set; }

//                     public NodeAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous, AcrossItem member)
//                         : base(entry, nextPosition, previous)
//                     {
//                         this.Member = member;
//                     }

//                     protected override NodeOrToken? ToMember()
//                         => this.Member.AsNode();

//                     protected override IEnumerable<NodeOrToken?> GetMembers()
//                         => this.Previous.Select(p => p.GetMembers().Concat(this.ToMember().Return()), () => this.ToMember().Return());

//                     public override Node AsNode()
//                         => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol, this.GetMembers());
//                 }

//                 private sealed class EmptyAcrossItem : AcrossItem
//                 {
//                     // instance members

//                     public EmptyAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous)
//                         : base(entry, nextPosition, previous)
//                     {
//                     }

//                     protected override NodeOrToken? ToMember()
//                         => null;

//                     protected override IEnumerable<NodeOrToken?> GetMembers()
//                     {
//                         yield break;
//                     }

//                     public override Node AsNode()
//                         => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol);
//                 }

//                 private sealed class TokenAcrossItem : AcrossItem
//                 {
//                     // instance members
//                     public Token Token { get; private set; }

//                     public TokenAcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous, Token token)
//                         : base(entry, nextPosition, previous)
//                     {
//                         this.Token = token;
//                     }

//                     protected override NodeOrToken? ToMember()
//                         => this.Token;

//                     protected override IEnumerable<NodeOrToken?> GetMembers()
//                         => this.Previous.Select(p => p.GetMembers().Concat(this.ToMember().Return()), () => this.ToMember().Return());

//                     public override Node AsNode()
//                         => new Node(this.Entry.Rule.Id, this.Entry.Rule.Symbol, this.GetMembers());
//                 }

//                 // instance members
//                 public FinalChart.Entry         Entry           { get; private set; }
//                 public FinalChart.Position      NextPosition    { get; private set; }
//                 public Option<AcrossItem>       Previous        { get; private set; }

//                 protected AcrossItem(FinalChart.Entry entry, FinalChart.Position nextPosition, Option<AcrossItem> previous)
//                 {
//                     this.Entry          = entry;
//                     this.NextPosition   = nextPosition;
//                     this.Previous       = previous;
//                 }

//                 public AcrossItem NextAcrossNode(FinalChart.Position nextPosition, AcrossItem member)
//                     => new NodeAcrossItem(this.Entry, nextPosition, this, member);
//                 public AcrossItem NextAcrossEmpty(FinalChart.Entry empty)
//                     => new EmptyAcrossItem(this.Entry, this.NextPosition, this);
//                 public AcrossItem NextAcrossToken(FinalChart.Position nextPosition, Token token)
//                     => new TokenAcrossItem(this.Entry, nextPosition, this, token);

//                 protected abstract NodeOrToken? ToMember();

//                 protected abstract IEnumerable<NodeOrToken?> GetMembers();

//                 /// <summary>
//                 /// Returns the complete node represented by this AcrossItem.
//                 /// </summary>
//                 /// <returns></returns>
//                 public abstract Node AsNode();
//             }

//             private class DownItem
//             {
//                 // instance members
//                 public Option<DownItem>  Previous { get; private set; }
//                 public FinalChart.Entry  Current  { get; private set; }

//                 private DownItem(Option<DownItem> previous, FinalChart.Entry current)
//                 {
//                     this.Previous = previous;
//                     this.Current  = current;
//                 }

//                 public DownItem(FinalChart.Entry current) : this(Option<DownItem>.None, current) { }
//                 public DownItem WithNext(FinalChart.Entry current) => new DownItem(this, current);
//                 public Boolean Contains(FinalChart.Entry entry) 
//                     => this.Current.Equals(entry) || this.Previous.Select(p => p.Contains(entry)).Or(false);
//             }


//             private abstract class BuildCall
//             {
//                 public BuildStates          CallState   { get; private set; }
//                 public BuildStates          ReturnState { get; private set; }
//                 public FinalChart.Entry     Entry       { get; private set; }
//                 public FinalChart.Position  Position    { get; private set; }

//                 public Identifier           SearchId    { get; set; }
//                 public FinalChart.Entry     Match       { get; set; }
//                 public Int32                MatchIndex  { get; set; }
//                 public Option<AcrossItem>   Result      { get; set; }

//                 protected BuildCall(BuildStates call, BuildStates @return, FinalChart.Entry entry, FinalChart.Position position)
//                 {
//                     this.CallState   = call;
//                     this.ReturnState = @return;
//                     this.Entry       = entry;
//                     this.Position    = position;
//                     this.MatchIndex  = -1;
//                 }

//                 public void SetCallState(BuildStates newCallState) { this.CallState = newCallState; }
//             }

//             private sealed class AcrossCall : BuildCall
//             {
//                 public Option<AcrossItem>   Previous    { get; private set; }
//                 public Int32                CurrentRule { get; private set; }
//                 public Int32                LengthLeft  { get; private set; }

//                 public AcrossCall(BuildStates call, BuildStates @return, FinalChart.Entry entry, FinalChart.Position position, Int32 currentRule, Int32 lengthLeft, AcrossItem? previous = null)
//                     : base(call, @return, entry, position)
//                 {
//                     this.Previous       = previous ?? Option<AcrossItem>.None;
//                     this.CurrentRule    = currentRule;
//                     this.LengthLeft     = lengthLeft;
//                 }
//             }

//             private sealed class DownCall : BuildCall
//             {
//                 public Option<DownItem> Above { get; set; }

//                 public DownCall(BuildStates call, BuildStates @return, FinalChart.Entry entry, FinalChart.Position position, DownItem? above = null)
//                     : base(call, @return, entry, position)
//                 {
//                     this.Above = above ?? Option<DownItem>.None;
//                 }
//             }


//             // The function of BuildAcross is to create string of one or more AcrossItem objects that represents a valid build
//             // of a rule from chart entries.
//             // Note: When BuildAcross calls itself, the entry does not change, the currentRule changes.

            
//             private struct BuildStates : IEquatable<BuildStates>
//             {
//                 // static members
//                 public static readonly BuildStates ACROSS_CALL = new BuildStates(0);
//                 public static readonly BuildStates ACROSS_MATCH = new BuildStates(1);
//                 public static readonly BuildStates ACROSS_MATCH_DOWN_CHECK = new BuildStates(2);
//                 public static readonly BuildStates ACROSS_MATCH_ACROSS_CHECK = new BuildStates(3);
//                 public static readonly BuildStates DOWN_CALL = new BuildStates(4);
//                 public static readonly BuildStates DOWN_MATCH = new BuildStates(5);
//                 public static readonly BuildStates DOWN_MATCH_DOWN_CHECK = new BuildStates(6);
//                 public static readonly BuildStates DOWN_MATCH_ACROSS_CHECK = new BuildStates(7);
//                 public static readonly BuildStates RETURN = new BuildStates(8);
//                 public static readonly BuildStates FINAL = new BuildStates(9);

//                 public static implicit operator Int32(BuildStates buildState) => buildState._Value;
//                 public static Boolean operator ==(BuildStates lhs, BuildStates rhs) => lhs.Equals(rhs);
//                 public static Boolean operator !=(BuildStates lhs, BuildStates rhs) => !lhs.Equals(rhs);

//                 // instance members
//                 private Int32 _Value;
//                 private BuildStates(Int32 value) { this._Value = value; }

//                 public Boolean Equals(BuildStates other)
//                     => this._Value.Equals(other._Value);
//                 public override Int32 GetHashCode()
//                     => this._Value.GetHashCode();
//                 public override Boolean Equals(Object? other)
//                     => other is BuildStates bs ? this.Equals(bs) : false;
//             }


//             private static Option<AcrossItem> Build(FinalChart chart, Token[] input, FinalChart.Entry initialEntry, FinalChart.Position initialPosition)
//             {
//                 var stack  = new Stack<BuildCall>();
//                 var across = new AcrossCall(BuildStates.ACROSS_CALL, BuildStates.FINAL, initialEntry, initialPosition, 0, initialEntry.Number);
//                 var down   = (DownCall?)null;
//                 var build  = (BuildCall)across;

//                 var downCache = new Dictionary<DownKey, Option<AcrossItem>>();

//                 Boolean isValidAcross(AcrossCall checkAcross, FinalChart.Entry checkFind)
//                 {
//                     if (checkAcross.SearchId   != checkFind.Rule.Id ||
//                         checkAcross.LengthLeft <  checkFind.Number)
//                         return false;

//                     var checkRule = checkAcross.Entry.Rule;

//                     if ((checkAcross.CurrentRule + 1) == checkRule.DefinitionIds.Count)
//                         return checkFind.Number == checkAcross.LengthLeft;

//                     // this implementation could be expanded to check more entries, but the 
//                     // downside would be that it would need to look at permutations
//                     var checkSymbolId = checkRule.DefinitionIds[checkAcross.CurrentRule + 1];
//                     var checkPosition = chart[checkAcross.Position.Index + checkFind.Number];

//                     return chart.Grammar.IsTerminal(checkSymbolId)
//                            ? input[checkPosition.Index].Id == checkSymbolId
//                            : checkPosition.Any(e => e.Rule.Id == checkSymbolId);
//                 }

//                 Boolean isValidDown(DownCall checkDown, FinalChart.Entry checkFind)
//                 {
//                     if (checkDown.SearchId     != checkFind.Rule.Id ||
//                         checkDown.Entry.Number <  checkFind.Number  ||
//                         down.Above.Value.Contains(checkFind)          )
//                         return false;

//                     var checkRule = checkDown.Entry.Rule;

//                     if (checkRule.DefinitionIds.Count == 1)
//                         return (checkDown.Entry.Number == checkFind.Number);

//                     var checkSymbolId = checkRule.DefinitionIds[1];
//                     var checkPosition = chart[checkDown.Position.Index + checkFind.Number];

//                     return chart.Grammar.IsTerminal(checkSymbolId)
//                            ? input[checkPosition.Index].Id == checkSymbolId
//                            : checkPosition.Any(e => e.Rule.Id == checkSymbolId);
//                 }

//                 void CallAcross(BuildStates @return, FinalChart.Entry entry, FinalChart.Position position, Int32 currentRule, Int32 lengthLeft, AcrossItem? previous = null)
//                 {
//                     stack.Push(build);

//                     across = new AcrossCall(BuildStates.ACROSS_CALL, @return, entry, position, currentRule, lengthLeft, previous);
//                     build  = across;
//                 }

//                 void CallDown(BuildStates @return, FinalChart.Entry callEntry, FinalChart.Position callPosition, DownItem? above = null)
//                 {
//                     stack.Push(build);

//                     down  = new DownCall(BuildStates.DOWN_CALL, @return, callEntry, callPosition, above);
//                     build = down;
//                 }

//                 void CallReturn()
//                 {
//                     if (stack.Count > 0)
//                     {
//                         var prior = build;

//                         build = stack.Pop();

//                         if (build is AcrossCall)
//                             across = build as AcrossCall;
//                         else
//                             down = build as DownCall;

//                         build.Result = prior.Result;
//                         build.SetCallState(prior.ReturnState);
//                     }
//                     else
//                     {
//                         build.SetCallState(build.ReturnState);
//                     }
//                 }

//                 var stateLogic = new Action[BuildStates.FINAL];

//                 stateLogic[BuildStates.ACROSS_CALL] = () =>
//                 {
//                     if (across.Entry.Rule.IsEmpty)
//                     {
//                         across.Result = across.Entry.Number == 0 ? AcrossItem.StartWithEmpty(across.Entry, across.Position) : Option<AcrossItem>.None;
//                         CallReturn();
//                     }
//                     else if (across.CurrentRule >= across.Entry.Rule.DefinitionIds.Count)
//                     {
//                         across.Result = across.Previous;
//                         CallReturn();
//                     }
//                     else
//                     {
//                         var id = across.Entry.Rule.DefinitionIds[across.CurrentRule];

//                         if (chart.Grammar.IsTerminal(id))
//                         {
//                             var token        = input[across.Position.Index];
//                             var nextPosition = chart[across.Position.Index + 1];

//                             if (token.Id == id)
//                             {
//                                 var previous = across.Previous.Select(p => p.NextAcrossToken(nextPosition, token),
//                                                                         () => AcrossItem.StartWithToken(across.Entry, nextPosition, token));

//                                 CallAcross(BuildStates.RETURN, across.Entry, nextPosition, across.CurrentRule + 1, across.LengthLeft - 1, previous);
//                             }
//                             else
//                             {
//                                 across.Result = Option<AcrossItem>.None;
//                                 CallReturn();
//                             }
//                         }
//                         else
//                         {
//                             across.SearchId = id;
//                             across.SetCallState(BuildStates.ACROSS_MATCH);
//                         }
//                     }
//                 };

//                 stateLogic[BuildStates.ACROSS_MATCH] = () =>
//                 {
//                     across.MatchIndex = across.MatchIndex + 1; // Note: MatchIndex is initialized to -1

//                     if (across.MatchIndex < across.Position.Count)
//                     {
//                         var find = across.Position[across.MatchIndex];

//                         if (isValidAcross(across, find))
//                         {
//                             across.Match = find;

//                             var downKey = new DownKey(across.Match, across.Position);

//                             if (downCache.ContainsKey(downKey))
//                             {
// #if DEBUG
//                                 _DownCallSaves++;
// #endif

//                                 across.Result = downCache[downKey];
//                                 across.SetCallState(BuildStates.ACROSS_MATCH_DOWN_CHECK);
//                             }
//                             else
//                             {
//                                 CallDown(BuildStates.ACROSS_MATCH_DOWN_CHECK, find, across.Position);
//                             }
//                         }
//                     }
//                     else
//                     {
//                         across.Result = Option<AcrossItem>.None;
//                         CallReturn();
//                     }
//                 };

//                 stateLogic[BuildStates.ACROSS_MATCH_DOWN_CHECK] = () =>
//                 {
//                     var downKey = new DownKey(across.Match, across.Position);

//                     if (!downCache.ContainsKey(downKey))
//                         downCache.Add(downKey, across.Result);

//                     if (across.Result.IsSome)
//                     {
//                         var nextPosition = across.Result.Value.NextPosition;
//                         var previous = across.Previous.Select(p => p.NextAcrossNode(nextPosition, across.Result.Value),
//                                                                 () => AcrossItem.StartWithNode(across.Entry, nextPosition, across.Result.Value));

//                         CallAcross(BuildStates.ACROSS_MATCH_ACROSS_CHECK, across.Entry, nextPosition, across.CurrentRule + 1, across.LengthLeft - across.Match.Number, previous);
//                     }
//                     else
//                     {
//                         across.SetCallState(BuildStates.ACROSS_MATCH);
//                     }
//                 };

//                 stateLogic[BuildStates.ACROSS_MATCH_ACROSS_CHECK] = () =>
//                 {
//                     if (across.Result.IsSome)
//                         CallReturn();
//                     else
//                         across.SetCallState(BuildStates.ACROSS_MATCH);
//                 };

//                 stateLogic[BuildStates.DOWN_CALL] = () =>
//                 {
//                     if (down.Entry.Rule.IsEmpty)
//                     {
//                         down.Result = down.Entry.Number == 0 ? AcrossItem.StartWithEmpty(down.Entry, down.Position) : Option<AcrossItem>.None;
//                         CallReturn();
//                     }
//                     else
//                     {
//                         var id = down.Entry.Rule.DefinitionIds[0];

//                         if (chart.Grammar.IsTerminal(id))
//                         {
//                             var token        = input[down.Position.Index];
//                             var nextPosition = chart[down.Position.Index + 1];

//                             if (token.Id == id)
//                             {
//                                 CallAcross(BuildStates.RETURN, down.Entry, nextPosition, 1, down.Entry.Number - 1, AcrossItem.StartWithToken(down.Entry, nextPosition, token));
//                             }
//                             else
//                             {
//                                 down.Result = Option<AcrossItem>.None;
//                                 CallReturn();
//                             }
//                         }
//                         else
//                         {
//                             down.SearchId = id;
//                             down.Above = down.Above.Select(p => p.WithNext(down.Entry), () => new DownItem(down.Entry));
//                             down.SetCallState(BuildStates.DOWN_MATCH);
//                         }
//                     }
//                 };

//                 stateLogic[BuildStates.DOWN_MATCH] = () =>
//                 {
//                     down.MatchIndex = down.MatchIndex + 1; // Note: MatchIndex is initialized to -1

//                     if (down.MatchIndex < down.Position.Count)
//                     {
//                         var find = down.Position[down.MatchIndex];

//                         if (isValidDown(down, find))
//                         {
//                             down.Match = find;

//                             CallDown(BuildStates.DOWN_MATCH_DOWN_CHECK, find, down.Position, down.Above.Value);
//                         }
//                     }
//                     else
//                     {
//                         down.Result = Option<AcrossItem>.None;
//                         CallReturn();
//                     }
//                 };

//                 stateLogic[BuildStates.DOWN_MATCH_DOWN_CHECK] = () =>
//                 {
//                     if (down.Result.IsSome)
//                     {
//                         var nextPosition = down.Result.Value.NextPosition;

//                         CallAcross(BuildStates.DOWN_MATCH_ACROSS_CHECK, down.Entry, nextPosition, 1, down.Entry.Number - down.Match.Number, AcrossItem.StartWithNode(down.Entry, nextPosition, down.Result.Value));
//                     }
//                     else
//                     {
//                         down.SetCallState(BuildStates.DOWN_MATCH);
//                     }
//                 };

//                 stateLogic[BuildStates.DOWN_MATCH_ACROSS_CHECK] = () =>
//                 {
//                     if (down.Result.IsSome)
//                         CallReturn();
//                     else
//                         down.SetCallState(BuildStates.DOWN_MATCH);
//                 };

//                 stateLogic[BuildStates.RETURN] = () =>
//                 {
//                     CallReturn();
//                 };

//                 while (build.CallState != BuildStates.FINAL)
//                 {
//                     stateLogic[build.CallState]();
//                 }

//                 return across.Result;
//             }


//             public static Option<Node> Create(FinalChart chart, Token[] input)
//             {
//                 var id      = chart.Grammar.StartId;
//                 var symbol  = chart.Grammar.StartSymbol;
//                 var result  = Option<Node>.None;

//                 if (chart.Length > 0)
//                 {
//                     foreach (var entry in chart[0].Where(e => e.Rule.Id == id))
//                     {
//                         if (entry.Rule.IsEmpty && entry.Number == 0)
//                             continue;

//                         var check = Build(chart, input, entry, chart[0]);

//                         if (check.IsSome)
//                             return check.Value.AsNode();

//                         if (result.IsSome)
//                             break;
//                     }
//                 }

//                 return result;
//             }

//         }

    }

}
