using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing.Building;

internal static class Build
{

    internal static Option<AcrossItem> Across(InverseChart chart, Token[] input, ChartEntry initialEntry, Int32 initialPosition)
    {
        var stack   = new Stack<BuildCall>();
        var across  = new AcrossCall(BuildStates.ACROSS_CALL, BuildStates.FINAL, initialEntry, initialPosition, 0, initialEntry.Number);
        var down    = (DownCall?)null;
        var build   = (BuildCall)across;

        var downCache = new Dictionary<DownKey, Option<AcrossItem>>();

        Boolean isValidAcross(AcrossCall checkAcross, ChartEntry checkFind)
        {
            if (checkAcross.SearchId != checkFind.Rule.Id ||
                checkAcross.LengthLeft < checkFind.Number)
                return false;

            var checkRule = checkAcross.Entry.Rule;

            if ((checkAcross.CurrentRule + 1) == checkRule.DefinitionIds.Count)
                return checkFind.Number == checkAcross.LengthLeft;

            // this implementation could be expanded to check more entries, but the 
            // downside would be that it would need to look at permutations
            var checkSymbolId = checkRule.DefinitionIds[checkAcross.CurrentRule + 1];
            var checkPosition = checkAcross.Position + checkFind.Number;

            return chart.Grammar.IsTerminal(checkSymbolId)
                    ? input[checkPosition].Id.IsEqual(checkSymbolId)
                    : chart.Entries(checkPosition).Any(e => e.Rule.Id == checkSymbolId);
        }

        Boolean isValidDown(DownCall checkDown, ChartEntry checkFind)
        {
            if (checkDown.SearchId != checkFind.Rule.Id ||
                checkDown.Entry.Number < checkFind.Number ||
                down.Above.Map(v => v.Contains(checkFind)).Or(false))
                return false;

            var checkRule = checkDown.Entry.Rule;

            if (checkRule.DefinitionIds.Count == 1)
                return (checkDown.Entry.Number == checkFind.Number);

            var checkSymbolId = checkRule.DefinitionIds[1];
            var checkPosition = checkDown.Position + checkFind.Number;

            return chart.Grammar.IsTerminal(checkSymbolId)
                    ? input[checkPosition].Id.IsEqual(checkSymbolId)
                    : chart.Entries(checkPosition).Any(e => e.Rule.Id == checkSymbolId);
        }

        void CallAcross(BuildStates @return, ChartEntry entry, Int32 position, Int32 currentRule, Int32 lengthLeft, AcrossItem? previous = null)
        {
            stack.Push(build);

            across = new AcrossCall(BuildStates.ACROSS_CALL, @return, entry, position, currentRule, lengthLeft, previous);
            build  = across;
        }

        void CallDown(BuildStates @return, ChartEntry callEntry, Int32 callPosition, DownItem? above = null)
        {
            stack.Push(build);

            down  = new DownCall(BuildStates.DOWN_CALL, @return, callEntry, callPosition, above);
            build = down;
        }

        void CallReturn()
        {
            if (stack.Count > 0)
            {
                var prior = build;

                build = stack.Pop();

                if (build is AcrossCall)
                    across = build as AcrossCall;
                else
                    down = build as DownCall;

                build.Result = prior.Result;
                build.SetCallState(prior.ReturnState);
            }
            else
            {
                build.SetCallState(build.ReturnState);
            }
        }

        var stateLogic = new Action[BuildStates.FINAL];

        stateLogic[BuildStates.ACROSS_CALL] = () =>
        {
            if (across.Entry.Rule.IsEmpty)
            {
                across.Result = across.Entry.Number == 0 ? AcrossItem.StartWithEmpty(across.Entry, across.Position).Some() : Option<AcrossItem>.None;
                CallReturn();
            }
            else if (across.CurrentRule >= across.Entry.Rule.DefinitionIds.Count)
            {
                across.Result = across.Previous;
                CallReturn();
            }
            else
            {
                var id = across.Entry.Rule.DefinitionIds[across.CurrentRule];

                if (chart.Grammar.IsTerminal(id))
                {
                    var token        = input[across.Position];
                    var nextPosition = across.Position + 1;

                    if (token.Id.IsEqual(id))
                    {
                        var previous = across.Previous.Match(p => p.NextAcrossToken(nextPosition, token),
                                                             () => AcrossItem.StartWithToken(across.Entry, nextPosition, token));

                        CallAcross(BuildStates.RETURN, across.Entry, nextPosition, across.CurrentRule + 1, across.LengthLeft - 1, previous);
                    }
                    else
                    {
                        across.Result = Option<AcrossItem>.None;
                        CallReturn();
                    }
                }
                else
                {
                    across.SearchId = id;
                    across.SetCallState(BuildStates.ACROSS_MATCH);
                }
            }
        };

        stateLogic[BuildStates.ACROSS_MATCH] = () =>
        {
            across.MatchIndex = across.MatchIndex + 1; // Note: MatchIndex is initialized to -1
            var entries = chart.Entries(across.Position);

            if (across.MatchIndex < entries.Count)
            {
                var find = entries[across.MatchIndex];

                if (isValidAcross(across, find))
                {
                    across.Match = find;

                    var downKey = new DownKey(across.Match, across.Position);

                    if (downCache.ContainsKey(downKey))
                    {
                        across.Result = downCache[downKey];
                        across.SetCallState(BuildStates.ACROSS_MATCH_DOWN_CHECK);
                    }
                    else
                    {
                        CallDown(BuildStates.ACROSS_MATCH_DOWN_CHECK, find, across.Position);
                    }
                }
            }
            else
            {
                across.Result = Option<AcrossItem>.None;
                CallReturn();
            }
        };

        stateLogic[BuildStates.ACROSS_MATCH_DOWN_CHECK] = () =>
        {
            var downKey = new DownKey(across.Match, across.Position);

            if (!downCache.ContainsKey(downKey))
                downCache.Add(downKey, across.Result);

            if (across.Result is Some<AcrossItem> someAcross)
            {
                var nextPosition = someAcross.Value.NextPosition;
                var previous = across.Previous.Match(p => p.NextAcrossNode(nextPosition, someAcross.Value),
                                                     () => AcrossItem.StartWithNode(across.Entry, nextPosition, someAcross.Value));

                CallAcross(BuildStates.ACROSS_MATCH_ACROSS_CHECK, across.Entry, nextPosition, across.CurrentRule + 1, across.LengthLeft - across.Match.Number, previous);
            }
            else
            {
                across.SetCallState(BuildStates.ACROSS_MATCH);
            }
        };

        stateLogic[BuildStates.ACROSS_MATCH_ACROSS_CHECK] = () =>
        {
            if (across.Result is Some<AcrossItem>)
                CallReturn();
            else
                across.SetCallState(BuildStates.ACROSS_MATCH);
        };

        stateLogic[BuildStates.DOWN_CALL] = () =>
        {
            if (down.Entry.Rule.IsEmpty)
            {
                down.Result = down.Entry.Number == 0
                              ? AcrossItem.StartWithEmpty(down.Entry, down.Position).Some<AcrossItem>()
                              : Option<AcrossItem>.None;
                CallReturn();
            }
            else
            {
                var id = down.Entry.Rule.DefinitionIds[0];

                if (chart.Grammar.IsTerminal(id))
                {
                    var token        = input[down.Position];
                    var nextPosition = down.Position + 1;

                    if (token.Id.IsEqual(id))
                    {
                        CallAcross(BuildStates.RETURN, down.Entry, nextPosition, 1, down.Entry.Number - 1, AcrossItem.StartWithToken(down.Entry, nextPosition, token));
                    }
                    else
                    {
                        down.Result = Option<AcrossItem>.None;
                        CallReturn();
                    }
                }
                else
                {
                    down.SearchId = id;
                    down.Above = down.Above.Match(p => p.WithNext(down.Entry).Some(),
                                                  () => new DownItem(down.Entry).Some());
                    down.SetCallState(BuildStates.DOWN_MATCH);
                }
            }
        };

        stateLogic[BuildStates.DOWN_MATCH] = () =>
        {
            down.MatchIndex = down.MatchIndex + 1; // Note: MatchIndex is initialized to -1
            var entries = chart.Entries(down.Position);

            if (down.MatchIndex < entries.Count)
            {
                var find = entries[down.MatchIndex];

                if (isValidDown(down, find))
                {
                    down.Match = find;

                    CallDown(BuildStates.DOWN_MATCH_DOWN_CHECK, find, down.Position, down.Above.Match(v => v, () => (DownItem?)null));
                }
            }
            else
            {
                down.Result = Option<AcrossItem>.None;
                CallReturn();
            }
        };

        stateLogic[BuildStates.DOWN_MATCH_DOWN_CHECK] = () =>
        {
            if (down.Result is Some<AcrossItem> someAcross)
            {
                var nextPosition = someAcross.Value.NextPosition;

                CallAcross(BuildStates.DOWN_MATCH_ACROSS_CHECK, down.Entry, nextPosition, 1, down.Entry.Number - down.Match.Number, AcrossItem.StartWithNode(down.Entry, nextPosition, someAcross.Value));
            }
            else
            {
                down.SetCallState(BuildStates.DOWN_MATCH);
            }
        };

        stateLogic[BuildStates.DOWN_MATCH_ACROSS_CHECK] = () =>
        {
            if (down.Result is Some<AcrossItem>)
                CallReturn();
            else
                down.SetCallState(BuildStates.DOWN_MATCH);
        };

        stateLogic[BuildStates.RETURN] = () =>
        {
            CallReturn();
        };

        while (build.CallState != BuildStates.FINAL)
        {
            stateLogic[build.CallState]();
        }

        return across.Result;
    }


    public static Option<Node> Create(InverseChart chart, Token[] input)
    {
        var startId = chart.Grammar.StartId;

        if (chart.Data.Count > 0)
        {
            foreach (var entry in chart.Entries(0).Where(e => e.Rule.Id == startId))
            {
                if (entry.Rule.IsEmpty && entry.Number == 0)
                    continue;

                var check = Build.Across(chart, input, entry, 0);

                if (check is Some<AcrossItem> someAcross)
                    return someAcross.Value.AsNode().Some();
            }
        }

        return Option<Node>.None;
    }

}
