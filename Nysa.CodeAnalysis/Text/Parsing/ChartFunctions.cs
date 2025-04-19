using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;
using Nysa.Text.Parsing.Building;

namespace Nysa.Text.Parsing;

public static class ChartFunctions
{
    private static IReadOnlyList<ChartEntry> _EmptyEntries = new List<ChartEntry>();

    private static void AddUnique(this List<ChartEntry>?[] chart, Int32 index, ChartEntry entry)
    {
        if (chart[index] == null)
        {
            chart[index] = new List<ChartEntry>();
            // line above protects against null at chart[index]
            #pragma warning disable CS8602
            chart[index].Add(entry);
            #pragma warning restore CS8602
        }
        // primary if above protects against null at chart[index]
        #pragma warning disable CS8602
        else if (chart[index].IndexOf(entry) < 0)
        {
            chart[index].Add(entry);
        }
        #pragma warning restore CS8602
    }

    private static void AddRaw(this List<ChartEntry>?[] chart, Int32 index, ChartEntry entry)
    {
        if (chart[index] == null)
            chart[index] = new List<ChartEntry>();

        // if above protects against null at chart[index]
        #pragma warning disable CS8602
        chart[index].Add(entry);
        #pragma warning restore CS8602
    }

    public static IReadOnlyList<ChartEntry> Entries(this BasicChart @this, Int32 index)
        => index >= 0 && index <= @this.Data.Count
           ? @this.Data[index] ?? _EmptyEntries
           : _EmptyEntries;

    private static IEnumerable<ChartEntry> EntriesAt(this List<ChartEntry>?[] chart, Int32 index)
        => chart[index] ?? _EmptyEntries;

    public static IEnumerable<ChartEntry> Where(this ParseChart chart, Int32 index, Func<ChartEntry, Boolean> predicate)
        => chart.Entries(index).Where(predicate);

    private static Boolean NotNullAnd(this IReadOnlyList<ChartEntry>? @this, Func<IReadOnlyList<ChartEntry>, Boolean> also)
        => @this != null && also(@this);

    public static Boolean IsIncomplete(this ParseChart @this)
        => !@this.Entries(@this.Data.Count - 1).Any(entry => entry.Rule.Symbol == @this.Grammar.StartSymbol && entry.Number == 0);
    public static Boolean IsIncomplete(this InverseChart @this)
        => !@this.Entries(0).Any(entry => entry.Rule.Symbol == @this.Grammar.StartSymbol);

    public static string ToString(this BasicChart @this)
    {
        var build = new StringBuilder();

        for (Int32 i = 0; i < @this.Data.Count; i++)
        {
            build.AppendLine($"Position: [{i}]");

            var entries = @this.Entries(i);

            foreach (var entry in entries)
                build.AppendLine(String.Concat("   ", entry.ToString()));
        }

        return build.ToString();
    }

    public static ParseChart CreateChart(this Grammar grammar, Token[] input)
    {
        // NOTE: The chart is always one position greater than the length of the input.
        var data = new List<ChartEntry>?[input.Length + 1];

        if (grammar.IsTerminal(grammar.StartId))
            return new ParseChart(grammar, data);

        foreach (var rule in grammar.Rules(grammar.StartId))
            data.AddRaw(0, new ChartEntry(rule, 0, 0));

        for (Int32 p = 0; p < input.Length; p++)
        {
            for (Int32 e = 0; e < data[p]?.Count; e++)
            {
                var entry       = data[p][e];
                var symbolId    = entry.NextRuleId;

                if (symbolId.IsNone) // test for completion
                {
                    var entries = data[entry.Number] ?? _EmptyEntries;

                    for (Int32 i = 0; i < entries.Count; i++)
                    {
                        if (!entries[i].NextRuleId.IsNone && entries[i].NextRuleId == entry.Rule.Id)
                            data.AddUnique(p, entries[i].AsNextEntry());
                    }
                }
                else if (grammar.IsTerminal(symbolId)) // test for terminal
                {
                    if (input[p].Id.IsEqual(symbolId))
                        data.AddRaw(p + 1, entry.AsNextEntry());
                }
                else // use rules to anticipate input and completion
                {
                    foreach (var rule in grammar.Rules(symbolId))
                    {
                        data.AddUnique(p, new ChartEntry(rule, p, 0));

                        if (grammar.NullableIds.Contains(symbolId))
                            data.AddUnique(p, entry.AsNextEntry());
                    }
                }
            } // foreach entry in position
        } // foreach position in input/chart

        return new ParseChart(grammar, data);
    }

    public static ParseChart CreateChart(this Grammar grammar, Token[] input, ICreateChartListener listener)
    {
        // NOTE: The chart is always one position greater than the length of the input.
        var data = new List<ChartEntry>?[input.Length + 1];

        var chart = new ParseChart(grammar, data);

        if (grammar.IsTerminal(grammar.StartId))
            return new ParseChart(grammar, data);

        foreach (var rule in grammar.Rules(grammar.StartId))
        {
            data.AddRaw(0, new ChartEntry(rule, 0, 0));
            listener.ChartChanged(0, chart);
        }

        for (Int32 p = 0; p < input.Length; p++)
        {
            for (Int32 e = 0; e < data[p]?.Count; e++)
            {
                var entry       = data[p][e];
                var symbolId    = entry.NextRuleId;

                if (symbolId.IsNone) // test for completion
                {
                    var entries = data[entry.Number] ?? _EmptyEntries;

                    for (Int32 i = 0; i < entries.Count; i++)
                    {
                        if (!entries[i].NextRuleId.IsNone && entries[i].NextRuleId == entry.Rule.Id)
                        {
                            data.AddUnique(p, entries[i].AsNextEntry());
                            listener.ChartChanged(p, chart);
                        }
                    }
                }
                else if (grammar.IsTerminal(symbolId)) // test for terminal
                {
                    if (input[p].Id.IsEqual(symbolId))
                    {
                        data.AddRaw(p + 1, entry.AsNextEntry());
                        listener.ChartChanged(p + 1, chart);
                    }
                }
                else // use rules to anticipate input and completion
                {
                    foreach (var rule in grammar.Rules(symbolId))
                    {
                        data.AddUnique(p, new ChartEntry(rule, p, 0));
                        listener.ChartChanged(p, chart);

                        if (grammar.NullableIds.Contains(symbolId))
                        {
                            data.AddUnique(p, entry.AsNextEntry());
                            listener.ChartChanged(p, chart);
                        }
                    }
                }
            } // foreach entry in position
        } // foreach position in input/chart

        return chart;
    }

    public static InverseChart InverseChart(this ParseChart @this)
    {
        var data = new List<ChartEntry>?[@this.Data.Count];

        for (var i = @this.Data.Count - 1; i >= 0; i--)
            foreach (var entry in (@this.Data[i] ?? _EmptyEntries).Where(e => e.IsComplete))
                data.AddRaw(entry.Number, entry.AsInverted(i));

        return new InverseChart(@this.Grammar, data);
    }

    private static (Int32 ErrorIndex, String Rules) ErrorInfo(this ParseChart chart)
    {
        var errIndex  = 0;
        var errPoint  = chart.Entries(0);

        // Note: The last token should always be an end-of-input marker.
        // Find the first spot in the chart where there are no entries.
        while (errIndex < chart.Data.Count - 1 && errPoint.Count > 0)
        {
            errIndex++;
            errPoint = chart.Entries(errIndex);
        }

        // we're not at the start and we stopped where there are no entries...
        if (errIndex > 0 && errPoint.Count < 1)
        {
            // back up one
            errIndex--;
            errPoint = chart.Entries(errIndex);
        }

        // formulate rules string from outstanding rules where the next rule is a terminal
        // these are the expected terminals the current token didn't match
        var rules = String.Join("\r\n", errPoint.Where(e =>    e.NextRuleId != Identifier.None
                                                            && chart.Grammar.IsTerminal(e.NextRuleId))
                                                .Select(t => t.ToString()));

        return (errIndex, rules);
    }

    public static (Int32 LineNumber, Int32 LineStart, Int32 LineLength) LineInfo(this Token @this, String source)
    {
        var position = 0;
        var lineIdx  = 0;
        var startIdx = 0;
        var endIdx   = @this.Span.Position + @this.Span.Length;

        while (position <= @this.Span.Position)
        {
            var newLineNext = source[position] == '\n';

            position++;

            if (newLineNext)
            {
                lineIdx++;
                startIdx = position;
            } 
        }

        return (lineIdx + 1, startIdx, (position - startIdx) + @this.Span.Length);
    }

    private static String ErrorMessage(this ParseChart @this, Token[] tokens, Int32 errorIndex)
        => @this.Grammar.IsValid(tokens[errorIndex].Id)
           ? "Unexpected symbol."
           : "Invalid symbol.";

    public static ParseException CreateError(this ParseChart chart, String source, Token[] tokens)
    {
        var (errIndex,
             errRules  ) = chart.ErrorInfo();
        var errToken     = tokens[errIndex];
        var (lineNumber,
             lineStart,
             lineLength) = errToken.LineInfo(source);

        return new ParseException(chart.ErrorMessage(tokens, errIndex),
                                  lineNumber,
                                  (lineLength - errToken.Span.Length),
                                  source.Substring(lineStart, lineLength),
                                  errRules);
    }

    // Given a set of node members and the identifier that has the RollupSiblings policy,
    // yield all the direct descendents having that same identifier.
    private static IEnumerable<Node> RolledUpSiblings(this Node sibling)
    {
        yield return sibling;

        foreach (var member in sibling.Members.Where(m => m.AsNode != null && m.AsNode.Id.Equals(sibling.Id)))
            foreach (var descendent in member.AsNode.RolledUpSiblings())
                yield return descendent;
    }

    private static IEnumerable<NodeOrToken> CollapsedMembers(this IEnumerable<NodeOrToken> members, NodePolicy parentPolicy, Grammar grammar)
    {
        foreach (var member in members)
        {
            if (member.AsNode != null)
            {
                var node   = member.AsNode;
                var policy = grammar.NodePolicy(node.Id);

                if (policy == NodePolicy.Remove)
                    continue;
                else if (policy == NodePolicy.Collapse)
                    foreach (var sub in member.AsNode.Members.CollapsedMembers(policy, grammar))
                        yield return sub;
                else if (policy == NodePolicy.CollapseSingle && member.AsNode.Members.Count == 1)
                    foreach (var sub in member.AsNode.Members.CollapsedMembers(policy, grammar))
                        yield return sub;
                else if (policy == NodePolicy.RemoveEmpty && member.AsNode.Members.Count == 0)
                    continue;
                else if (policy == NodePolicy.RollupSiblings)
                {
                    if (parentPolicy == NodePolicy.RollupSiblings)
                        continue; // don't return any of these under a parent with RollupSiblings

                    foreach (var sibling in node.RolledUpSiblings())
                        yield return sibling.Collapsed(grammar);
                }
                else // NodePolicy.Default
                    yield return node.Collapsed(grammar);
            }
            else
                yield return member; // terminals always have the Default policy (they're only removed as a result of parent removal)
        }
    }

    private static Node Collapsed(this Node node, Grammar grammar)
        => new Node(node.Id, node.Symbol, node.Members.CollapsedMembers(grammar.NodePolicy(node.Id), grammar));

    public static Option<Node> ToSyntaxTree(this InverseChart @this, Token[] tokens)
    {
        var id      = @this.Grammar.StartId;
        var symbol  = @this.Grammar.StartSymbol;
        var result  = Option<Node>.None;

        if (@this.Data.Count > 0)
        {
            var entries = @this.Entries(0);

            foreach (var entry in entries.Where(e => e.Rule.Id == id))
            {
                if (entry.Rule.IsEmpty && entry.Number == 0)
                    continue;

                var check = Build.Across(@this, tokens, entry, 0);

                if (check is Some<AcrossItem> someAcross)
                    return someAcross.Value.AsNode().Collapsed(@this.Grammar).Some();

                if (result is Some<Node>)
                    break;
            }
        }

        return result.Map(n => n.Collapsed(@this.Grammar));
    }

}
