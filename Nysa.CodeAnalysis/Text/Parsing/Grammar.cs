using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public class Grammar
    {
        // static members
        private static readonly Nysa.Text.Lexing.Rule _RuleCheck        = Take.AtStart.Then('<'.One()).Then(Take.Until('>'.One())).Where(s => s.Length > 2);
        private static readonly Nysa.Text.Lexing.Rule _CategoryCheck    = Take.AtStart.Then('{'.One()).Then(Take.Until('}'.One())).Where(s => s.Length > 2);

        public static Boolean IsLiteralSymbol(String symbol)
            => (   _RuleCheck.Function(Start.Span(symbol)) is LexMiss
                && _CategoryCheck.Function(Start.Span(symbol)) is LexMiss);
        public static Boolean IsRuleSymbol(String symbol)
            => _RuleCheck.Function(Start.Span(symbol)) is LexHit;
        public static Boolean IsCategorySymbol(String symbol)
            => _CategoryCheck.Function(Start.Span(symbol)) is LexHit;

        // We use a specific empty instance a list of rules to signify terminal symbols.
        internal static readonly List<GrammarRule>  TERMINAL       = new List<GrammarRule>();

        // instance members
        private SymbolIndex                                 _Index;
        private Dictionary<Identifier, SymbolDefinition>    _Rules;
        private HashSet<Identifier>                         _NullableIdSet;

        public String StartSymbol { get; private set; }
        public Identifier StartId { get; private set; }

        internal Grammar(String startSymbol, Identifier startId, SymbolIndex index, Dictionary<Identifier, SymbolDefinition> rulesIndex, HashSet<Identifier> nullables)
        {
            this.StartSymbol        = startSymbol;
            this.StartId            = startId;
            this._Index             = index;
            this._Rules             = rulesIndex;
            this._NullableIdSet     = nullables;
        }

        public IEnumerable<Identifier> GetIds(String[] symbols)
            => symbols.Select(s => this.Id(s));
        public IEnumerable<String> GetSymbols(IEnumerable<Identifier> ids)
            => ids.Select(i => this._Rules[i].Name);

        public Identifier Id(String symbol) => this._Index.Id(symbol);

        public String Symbol(Identifier id) => this._Index.Symbol(id);
        public Boolean IsValid(Identifier id) => (id != Identifier.None && this._Rules.ContainsKey(id));
        public Boolean IsValid(TokenIdentifier id) => id.Values().All(i => this.IsValid(i));
        public Boolean IsValid(String symbol) => this.IsValid(this.Id(symbol));
        public Boolean IsTerminal(Identifier id) => this.IsValid(id) && this._Rules[id].IsTerminal;

        public NodePolicy NodePolicy(Identifier id) => this._Rules[id].NodePolicy;

        public IReadOnlyList<GrammarRule> Rules(Identifier id)
            => this.IsValid(id) ? this._Rules[id].Variants : TERMINAL;

        public HashSet<Identifier> NullableIds
        {
            get => this._NullableIdSet;
        }

        public IEnumerable<String> LiteralSymbols()
            => this._Index.All.Where(kvp => IsLiteralSymbol(kvp.Symbol)).Select(l => l.Symbol);
        public IEnumerable<String> RuleSymbols()
            => this._Index.All.Where(kvp => IsRuleSymbol(kvp.Symbol)).Select(r => r.Symbol);
        public IEnumerable<String> CategorySymbols()
            => this._Index.All.Where(kvp => IsCategorySymbol(kvp.Symbol)).Select(c => c.Symbol);

        public override string ToString()
            => this._Rules
                   .Where(x => !x.Value.IsTerminal)
                   .SelectMany(r => r.Value.Variants.Select(v => (r.Value.Name, this.GetSymbols(v.DefinitionIds))))
                   .Select(y => $"<{y.Item1}> ::= {y.Item2.Aggregate(String.Empty, (a, s) => String.Concat(a, " <", s, ">"))}")
                   .Aggregate(String.Empty, (a, s) => String.Concat(a, s, "\r\n"));

    }

}
