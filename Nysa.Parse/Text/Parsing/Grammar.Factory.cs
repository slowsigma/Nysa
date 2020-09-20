using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {

        public partial class Factory
        {
            private static readonly Take.Node takeAlpha     = ((Some<Take.AnyOneNode>)"abcdefghijklmnopqrstuvwxyz".AnyOne(true)).Value;
            private static readonly Take.Node takeAlphaPlus = ((Some<Take.AnyOneNode>)"abcdefghijklmnopqrstuvwxyz0123456789_-".AnyOne(true)).Value;

            internal static readonly Take.Node TakeIdentifier = takeAlpha.Then(Take.While(takeAlphaPlus))
                                                                         .Where(s => s.Length > 0);

            internal static readonly Take.Node StrictRuleSymbol     = '<'.One(true)
                                                                         .Then(TakeIdentifier)
                                                                         .Then('>'.One(true));
            internal static readonly Take.Node StrictCategorySymbol = '{'.One(true)
                                                                         .Then(TakeIdentifier)
                                                                         .Then('}'.One(true));

            private static Int32 _NextEitherSuffix = 1;
            private static Int32 _NextRepeatSuffix = 1;

            public delegate ExplicitRule ExplicitDefine(String symbol, NodeRetentionType nodeRetention = NodeRetentionType.Keep);
            public delegate EitherRule   EitherDefine  (params Symbol[] definition);
            public delegate RepeatRule   RepeatDefine  (Symbol repeatItem);


            // instance members
            public String           StartSymbol { get; private set; }
            public ExplicitDefine   Define      { get; private set; }
            public EitherDefine     Either      { get; private set; }
            public RepeatDefine     Repeat      { get; private set; }
            public Symbol           SelfSymbol  => Symbol.Self;
            public Symbol           EndSymbol   => Symbol.End;

            private Dictionary<String, Rule> _Rules;

            public Factory(String startSymbol)
            {
                this.StartSymbol    = startSymbol;
                this._Rules         = new Dictionary<String, Rule>(StringComparer.OrdinalIgnoreCase);

                this.Define         = this.AddExplicit;
                this.Either         = this.AddEither;
                this.Repeat         = this.AddRepeat;
            }

            private ExplicitRule AddExplicit(String symbol, NodeRetentionType nodeRetention = NodeRetentionType.Keep)
            {
                if (!StrictRuleSymbol.Find(symbol.Start()).IsHit)
                    throw new ArgumentException($"Error in {symbol}. Rule symbols must begin with '<', contain one alpha character followed by zero, one, or more alpha, number, dash, or underscore characters and end with '>'.", nameof(symbol));
                if (this._Rules.ContainsKey(symbol))
                    throw new InvalidOperationException($"Error in {symbol}. This symbol is already defined.");

                var start = new Rule(symbol, nodeRetention);

                this._Rules.Add(start.Symbol, start);

                return new ExplicitRule(start);
            }

            private EitherRule AddEither(params Symbol[] definition)
            {
                var start = new Rule($"<`AnonymousEither{_NextEitherSuffix++}>", NodeRetentionType.Collapse)
                            .WithDefinition(false, definition);

                this._Rules.Add(start.Symbol, start);

                return new EitherRule(start);
            }

            private RepeatRule AddRepeat(Symbol repeatItem)
            {
                var start = new Rule($"<`AnonymousRepeat{_NextRepeatSuffix++}>", NodeRetentionType.Collapse);

                if (repeatItem.Value.Equals(Symbol.Self))
                    throw new ArgumentException($"Error in {start.Symbol}. The item of a repeat rule cannot be self referencing.", nameof(repeatItem));

                this._Rules.Add(start.Symbol, start);

                return new RepeatRule(start.WithDefinition(true, new Symbol[] { repeatItem, start.Symbol }));
            }

            public IEnumerable<Rule> Rules()
                => this._Rules.Select(kvp => kvp.Value);

            public IEnumerable<String> Symbols()
                => this._Rules.Select(r => r.Value.Symbol)
                       .Concat(this._Rules.SelectMany(r => r.Value.Definitions.SelectMany(d => d.Select(v => v.Value).ToArray())))
                       .Distinct(StringComparer.OrdinalIgnoreCase);

            public Suspect<Grammar> ToGrammar()
                => Grammar.Create(this);
        }

    }

}
