using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Text;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {
        // static members
        private static readonly Take.Node _RuleCheck        = Take.AtStart.Then(Take.One('<')).Then(Take.Until(Take.One('>'))).Where(s => s.Length > 2);
        private static readonly Take.Node _CategoryCheck    = Take.AtStart.Then(Take.One('{')).Then(Take.Until(Take.One('}'))).Where(s => s.Length > 2);

        public static Boolean IsLiteralSymbol(String symbol)
            => (   _RuleCheck.Find(Start.Span(symbol)) is LexMiss
                && _CategoryCheck.Find(Start.Span(symbol)) is LexMiss);
        public static Boolean IsRuleSymbol(String symbol)
            => _RuleCheck.Find(Start.Span(symbol)) is LexHit;
        public static Boolean IsCategorySymbol(String symbol)
            => _CategoryCheck.Find(Start.Span(symbol)) is LexHit;

        // We use a specific empty instance a list of rules to signify terminal symbols.
        private static readonly List<Rule>  TERMINAL       = new List<Rule>();

        // instance members
        private Dictionary<String, Identifier>              _SymbolIdentifier;
        private Dictionary<Identifier, SymbolDefinition>    _Rules;
        private HashSet<Identifier>                         _NullableIdSet;

        public String StartSymbol { get; private set; }
        public Identifier StartId { get; private set; }

        private Grammar(Builder builder)
        {
            this.StartSymbol = builder.StartSymbol;

            this._SymbolIdentifier = new Dictionary<String, Identifier>(StringComparer.OrdinalIgnoreCase);

            foreach (var keyIndex in builder.Symbols().Select((s, i) => (Symbol: s, Id: Identifier.FromInteger(i + 1))))
                this._SymbolIdentifier.Add(keyIndex.Symbol, keyIndex.Id);

            var definitions = builder.Rules()
                                     .ToDictionary(rk => rk.Symbol, 
                                                   rv => SymbolDefinition.Create(this, rv.Symbol, rv.Definitions),
                                                   StringComparer.OrdinalIgnoreCase);

            this._Rules     = this._SymbolIdentifier
                                  .Select(kvp => definitions.ContainsKey(kvp.Key)
                                                 ? definitions[kvp.Key]
                                                 : SymbolDefinition.CreateTerminal(kvp.Key))
                                  .ToDictionary(dk => this._SymbolIdentifier[dk.Name]);

            var known    = new HashSet<Identifier>(this._Rules.Where(r => r.Value.IsTerminal).Select(t => t.Key));
            var nullable = new HashSet<Identifier>(this._Rules.Where(r => !r.Value.IsTerminal && r.Value.Variants.Any(v => v.IsEmpty)).Select(n => n.Key));
            var unknown  = new HashSet<Identifier>(this._Rules.Where(r => !known.Contains(r.Key) && !nullable.Contains(r.Key)).Select(u => u.Key));

            var moreNull = unknown.Where(u => !this._Rules[u].IsTerminal && this._Rules[u].Variants.Any(d => d.DefinitionIds.All(s => nullable.Contains(s)))).ToArray();

            while (moreNull.Length > 0)
            {
                nullable.UnionWith(moreNull);
                unknown.ExceptWith(moreNull);
                moreNull = unknown.Where(u => !this._Rules[u].IsTerminal && this._Rules[u].Variants.Any(d => d.DefinitionIds.All(s => nullable.Contains(s)))).ToArray();
            }

            this._NullableIdSet = nullable;
            this.StartId = this.Id(this.StartSymbol);
        }

        public IEnumerable<Identifier> GetIds(String[] symbols)
            => symbols.Select(s => this.Id(s));
        public IEnumerable<String> GetSymbols(IEnumerable<Identifier> ids)
            => ids.Select(i => this._Rules[i].Name);

        public Identifier Id(String symbol)
        {
            Identifier id;
            return this._SymbolIdentifier.TryGetValue(symbol, out id) ? id : Identifier.None;
        }

        public String Symbol(Identifier id) => (id != Identifier.None && this._Rules.ContainsKey(id)) ? this._Rules[id].Name : "invalid-symbol-id";
        public Boolean IsValid(Identifier id) => (id != Identifier.None && this._Rules.ContainsKey(id));
        public Boolean IsValid(String symbol) => this.IsValid(this.Id(symbol));
        public Boolean IsTerminal(Identifier id) => this.IsValid(id) && this._Rules[id].IsTerminal;

        public IReadOnlyList<Rule> Rules(Identifier id)
            => this.IsValid(id) ? this._Rules[id].Variants : TERMINAL;

        public HashSet<Identifier> NullableIds
        {
            get => this._NullableIdSet;
        }

        public IEnumerable<String> LiteralSymbols()
            => this._SymbolIdentifier.Where(kvp => IsLiteralSymbol(kvp.Key)).Select(l => l.Key);
        public IEnumerable<String> RuleSymbols()
            => this._SymbolIdentifier.Where(kvp => IsRuleSymbol(kvp.Key)).Select(r => r.Key);
        public IEnumerable<String> CategorySymbols()
            => this._SymbolIdentifier.Where(kvp => IsCategorySymbol(kvp.Key)).Select(c => c.Key);

        public override string ToString()
            => this._Rules
                   .Where(x => !x.Value.IsTerminal)
                   .SelectMany(r => r.Value.Variants.Select(v => (r.Value.Name, this.GetSymbols(v.DefinitionIds))))
                   .Select(y => $"<{y.Item1}> ::= {y.Item2.Aggregate(String.Empty, (a, s) => String.Concat(a, " <", s, ">"))}")
                   .Aggregate(String.Empty, (a, s) => String.Concat(a, s, "\r\n"));

    }

}
