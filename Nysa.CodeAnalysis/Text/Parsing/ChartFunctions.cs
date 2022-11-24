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
    public static Chart Chart(this Grammar grammar, Token[] input)
    {
        var chart = new Chart(grammar, input.Length + 1);

        if (grammar.IsTerminal(grammar.StartId))
            return chart;

        foreach (var rule in grammar.Rules(grammar.StartId))
            chart[0].AddRaw(new ChartEntry(rule, 0, 0));

        for (Int32 p = 0; p < input.Length; p++)
        {
            for (Int32 e = 0; e < chart[p].Count; e++)
            {
                var entry       = chart[p][e];
                var symbolId    = entry.NextRuleId;

                if (symbolId.IsNone) // test for completion
                {
                    foreach (var incomplete in chart[entry.Number].GetEntries().Where(i => !i.NextRuleId.IsNone && i.NextRuleId == entry.Rule.Id))
                        chart[p].AddUnique(incomplete.AsNextEntry());
                }
                else if (grammar.IsTerminal(symbolId)) // test for terminal
                {
                    if (input[p].Id.IsEqual(symbolId))
                        chart[p + 1].AddRaw(entry.AsNextEntry());
                }
                else // use rules to anticipate input and completion
                {
                    foreach (var rule in grammar.Rules(symbolId))
                    {
                        chart[p].AddUnique(rule, p, 0);

                        if (grammar.NullableIds.Contains(symbolId))
                            chart[p].AddUnique(entry.AsNextEntry());
                    }
                }
            } // foreach entry in position
        } // foreach position in input/chart

        return chart;
    }

    public static Boolean IsIncomplete(this Chart @this)
        => !@this[@this.Length - 1].Any(entry => entry.Rule.Symbol == @this.Grammar.StartSymbol && entry.Number == 0);

    public static Boolean IsIncomplete(this FinalChart @this)
        => !@this[0].Any(entry => entry.Rule.Symbol == @this.Grammar.StartSymbol);



    private static IEnumerable<NodeOrToken> CollapsedMembers(this IEnumerable<NodeOrToken> members, Grammar grammar)
    {
        foreach (var member in members)
        {
            if (member.AsNode != null)
            {
                var policy = grammar.NodePolicy(member.AsNode.Id);

                if (policy == NodePolicy.Remove)
                    continue;
                if (policy == NodePolicy.Collapse)
                    foreach (var sub in member.AsNode.Members.CollapsedMembers(grammar))
                        yield return sub;
                else if (policy == NodePolicy.CollapseSingle && member.AsNode.Members.Count == 1)
                    foreach (var sub in member.AsNode.Members.CollapsedMembers(grammar))
                        yield return sub;
                else if (policy == NodePolicy.RemoveEmpty && member.AsNode.Members.Count == 0)
                    continue;
                else
                    yield return member.AsNode.Collapsed(grammar);
            }
            else
                yield return member; // terminals always have the Default policy (they may be removed as a result of a parent policy)
        }
    }

    private static Node Collapsed(this Node node, Grammar grammar)
        => new Node(node.Id, node.Symbol, node.Members.CollapsedMembers(grammar));

    public static Option<Node> ToSyntaxTree(this FinalChart @this, Token[] tokens)
    {
        var id      = @this.Grammar.StartId;
        var symbol  = @this.Grammar.StartSymbol;
        var result  = Option<Node>.None;

        if (@this.Length > 0)
        {
            foreach (var entry in @this[0].Where(e => e.Rule.Id == id))
            {
                if (entry.Rule.IsEmpty && entry.Number == 0)
                    continue;

                var check = Build.Across(@this, tokens, entry, @this[0]);

                if (check is Some<AcrossItem> someAcross)
                    return someAcross.Value.AsNode().Collapsed(@this.Grammar).Some();

                if (result is Some<Node>)
                    break;
            }
        }

        return result.Map(n => n.Collapsed(@this.Grammar));
    }

}
