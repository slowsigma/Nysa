﻿using System;
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

        private static class V1
        {

            public class AcrossLevel
            {
                // instance members
                public Option<AcrossLevel>  Previous { get; private set; }
                public Option<NodeOrToken>  Current  { get; private set; }
                public FinalChart.Position  Next     { get; private set; }

                private Node? _Node;

                private AcrossLevel(Option<AcrossLevel> previous, Option<NodeOrToken> current, FinalChart.Position next)
                {
                    this.Previous   = previous;
                    this.Current    = current;
                    this.Next       = next;

                    this._Node      = null;
                }

                public AcrossLevel(NodeOrToken current, FinalChart.Position next) : this(Option<AcrossLevel>.None, current, next) { }
                public AcrossLevel(FinalChart.Position next) : this(Option<AcrossLevel>.None, Option<NodeOrToken>.None, next) { }

                public AcrossLevel WithNext(NodeOrToken current, FinalChart.Position next)
                    => new AcrossLevel(this, current, next);

                private IEnumerable<NodeOrToken> Items()
                    => this.Current.Select(c => this.Previous.Select(p => p.Items().Concat(c.Return()), c.Return()),
                                           () => this.Previous.Select(p => p.Items()).OrValue(Enums.None<NodeOrToken>())
                                          );

                public Node AsNode(Identifier id, String symbol)
                {
                    if (this._Node == null)
                        this._Node = new Node(id, symbol, this.Items());

                    return this._Node;
                }
            }

            public class DownLevel
            {
                // instance members
                public Option<DownLevel> Previous  { get; private set; }
                public FinalChart.Entry  Current   { get; private set; }

                private Int32 PreviousCount { get => this.Previous.Select(p => 1 + p.PreviousCount).OrValue(0); }
                private IEnumerable<FinalChart.Entry> AllEntries()
                    => this.Previous.Select(p => p.AllEntries().Concat(this.Current.Return())).OrValue(Enums.None<FinalChart.Entry>());

                private DownLevel(Option<DownLevel> previous, FinalChart.Entry current)
                {
                    this.Previous = previous;
                    this.Current  = current;
                }

                public DownLevel(FinalChart.Entry current) : this(Option.None, current) { }

                public DownLevel WithNext(FinalChart.Entry current)
                    => new DownLevel(this, current);

                public Boolean Contains(FinalChart.Entry entry)
                    => this.Current == entry || this.Previous.Select(p => p.Contains(entry)).OrValue(false);
            }

            public static Option<AcrossLevel> BuildDown(Dictionary<DownKey, Option<AcrossLevel>> downIndex, FinalChart chart, FinalChart.Entry entry, FinalChart.Position position, Option<DownLevel> previous, Token[] input)
            {
                if (entry.Rule.IsEmpty)
                {
                    return entry.Number == 0
                           ? new AcrossLevel(chart[position.Index])
                           : Option<AcrossLevel>.None;
                }
                else
                {
                    var id = entry.Rule.DefinitionIds[0];

                    if (chart.Grammar.IsTerminal(id))
                    {
                        var token = input[position.Index];
                        var nextPosition = chart[position.Index + 1];

                        return (token.Id == id)
                               ? BuildAcross(downIndex, chart, entry, 1, entry.Number - 1, nextPosition, new AcrossLevel(token, nextPosition), input)
                               : Option<AcrossLevel>.None;
                    }
                    else
                    {
                        previous = previous.Select(p => p.WithNext(entry), () => new DownLevel(entry));

                        foreach (var match in position.Where(e => e.Rule.Id == id && e.Number <= entry.Number && !previous.Value.Contains(e)))
                        {
                            var down = BuildDown(downIndex, chart, match, position, previous, input);

                            if (down.IsSome)
                            {
                                var node  = down.Value.AsNode(match.Rule.Id, match.Rule.Symbol);
                                var next  = down.Value.Next;
                                var check = BuildAcross(downIndex, chart, entry, 1, entry.Number - match.Number, next, new AcrossLevel(node, next), input);

                                if (check.IsSome)
                                    return check;
                            }
                        }

                        return Option<AcrossLevel>.None;
                    }
                }
            }

            public static Option<AcrossLevel> BuildAcross(Dictionary<DownKey, Option<AcrossLevel>> downIndex, FinalChart chart, FinalChart.Entry entry, Int32 currentRule, Int32 lengthLeft, FinalChart.Position position, Option<AcrossLevel> previous, Token[] input)
            {
                if (entry.Rule.IsEmpty)
                {
                    return entry.Number == 0
                           ? previous.Select(p => p.WithNext(new Node(entry.Rule.Id, entry.Rule.Symbol), chart[position.Index]),
                                             () => new AcrossLevel(new Node(entry.Rule.Id, entry.Rule.Symbol), chart[position.Index]))
                           : Option<AcrossLevel>.None;
                }
                else if (currentRule < entry.Rule.DefinitionIds.Count)
                {
                    var id = entry.Rule.DefinitionIds[currentRule];

                    if (chart.Grammar.IsTerminal(id))
                    {
                        var token = input[position.Index];
                        var nextPosition = chart[position.Index + 1];

                        if (token.Id == id)
                        {
                            previous = previous.Select(p => p.WithNext(token, nextPosition),
                                                       () => new AcrossLevel(token, nextPosition));

                            return BuildAcross(downIndex, chart, entry, currentRule + 1, lengthLeft - 1, nextPosition, previous, input);
                        }
                        else
                            return Option<AcrossLevel>.None;
                    }
                    else
                    {
                        foreach (var match in position.Where(e => e.Rule.Id == id && e.Number <= lengthLeft))
                        {
                            if (match.Rule.IsEmpty && entry.Number == 0) // I think this is a mistake in V1 (entry.Number should be match.Number)
                            {
                                var check = BuildAcross(downIndex, chart, entry, currentRule, lengthLeft, position, previous.Select(p => p.WithNext(new Node(match.Rule.Id, match.Rule.Symbol), position)), input);

                                if (check.IsSome)
                                    return check;
                            }
                            else
                            {
                                var down = BuildDown(downIndex, chart, match, position, Option<DownLevel>.None, input);

                                if (down.IsSome)
                                {
                                    var node  = down.Value.AsNode(match.Rule.Id, match.Rule.Symbol);
                                    var next  = down.Value.Next;
                                    var check = BuildAcross(downIndex, chart, entry, currentRule + 1, lengthLeft - match.Number, next, previous.Select(p => p.WithNext(node, next), () => new AcrossLevel(node, next)), input);

                                    if (check.IsSome)
                                        return check;
                                }
                            }
                        }

                        return Option<AcrossLevel>.None;
                    }
                }
                else
                    return previous;
            }


            public static Option<Node> Create(FinalChart chart, Token[] input)
            {
                var id      = chart.Grammar.StartId;
                var symbol  = chart.Grammar.StartSymbol;
                var result  = Option<Node>.None;

                if (chart.Length > 0)
                {
                    foreach (var entry in chart[0].Where(e => e.Rule.Id == id))
                    {
                        if (entry.Rule.IsEmpty && entry.Number == 0)
                            continue;

                        var check = BuildAcross(new Dictionary<DownKey, Option<AcrossLevel>>(), chart, entry, 0, entry.Number, chart[0], Option<AcrossLevel>.None, input);

                        if (check.IsSome)
                            return check.Value.AsNode(entry.Rule.Id, entry.Rule.Symbol);

                        if (result.IsSome)
                            break;
                    }
                }

                return result;
            }

        }

    }

}
