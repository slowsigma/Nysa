using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {
        private Dictionary<String, Identifier>          _IdIndex;
        private Dictionary<Identifier, Rule>            _Rules;
        private HashSet<Identifier>                     _NullableIdSet;
        private Dictionary<Identifier, Lazy<String>>    _SymbolIndex;

        private Dictionary<Identifier, NodeRetentionType> _RetentionIndex;

        public String StartSymbol { get; private set; }
        public Identifier StartId { get; private set; }

        private Grammar(String startSymbol, Dictionary<String, Identifier> idIndex, Dictionary<Identifier, Rule> rules, HashSet<Identifier> nullables)
        {
            this._IdIndex       = idIndex;
            this._Rules         = rules;
            this._NullableIdSet = nullables;
            this._SymbolIndex   = new Dictionary<Identifier, Lazy<String>>();

            this.StartSymbol    = startSymbol;
            this.StartId        = this._IdIndex.Lookup(startSymbol, Identifier.None);

            this._RetentionIndex = rules.Where(r => !r.Value.IsTerminal && r.Value.NodeRetention != NodeRetentionType.Keep)
                                        .ToDictionary(k => k.Key,
                                                      v => v.Value.NodeRetention);
        }

        public IEnumerable<Identifier> GetIds(String[] symbols) => symbols.Select(s => this.GetId(s));
        public IEnumerable<String> GetSymbols(IEnumerable<Identifier> ids) => ids.Select(i => this._Rules[i].Symbol);
        public Identifier GetId(String symbol) => this._IdIndex.Lookup(symbol, Identifier.None);
        public Identifier GetId(Symbol symbol) => this._IdIndex.Lookup(symbol.ToString(), Identifier.None);
        public String GetSymbol(Identifier id) => (id != Identifier.None && this._Rules.ContainsKey(id)) ? this._Rules[id].Symbol : InvalidSymbol;
        public Boolean IsValid(Identifier id) => (id != Identifier.None && this._Rules.ContainsKey(id));
        public Boolean IsValid(String symbol) => this.IsValid(this.GetId(symbol));
        public Boolean IsTerminal(Identifier id) => this.IsValid(id) && this._Rules[id].IsTerminal;
        public IReadOnlyList<RuleVariant> Rules(Identifier id) => this.IsValid(id) ? this._Rules[id].Variants : Terminal;

        public HashSet<Identifier> NullableIds => this._NullableIdSet;
        public IEnumerable<String> LiteralSymbols() => this._IdIndex.Where(kvp => IsLiteralSymbol(kvp.Key)).Select(l => l.Key);
        public IEnumerable<String> RuleSymbols() => this._IdIndex.Where(kvp => IsRuleSymbol(kvp.Key)).Select(r => r.Key);
        public IEnumerable<String> CategorySymbols() => this._IdIndex.Where(kvp => IsCategorySymbol(kvp.Key)).Select(c => c.Key);

        public override string ToString()
            => this._Rules
                   .Where(x => !x.Value.IsTerminal)
                   .SelectMany(r => r.Value.Variants.Select(v => (r.Value.Symbol, this.GetSymbols(v.DefinitionIds))))
                   .Select(y => $"<{y.Item1}> ::= {y.Item2.Aggregate(String.Empty, (a, s) => String.Concat(a, " <", s, ">"))}")
                   .Aggregate(String.Empty, (a, s) => String.Concat(a, s, "\r\n"));



        private IEnumerable<NodeOrToken> FilteredMembers(Identifier parentId, IEnumerable<NodeOrToken> members)
        {
            foreach (var member in members)
            {
                if (member.IsNode)
                {
                    var retention = this._RetentionIndex.ContainsKey(member.AsNode.Id)
                                    ? this._RetentionIndex[member.AsNode.Id]
                                    : NodeRetentionType.Keep;

                    if (retention == NodeRetentionType.Remove)
                        continue;
                    if (retention == NodeRetentionType.RemoveEmpty && member.AsNode.Members.Count == 0)
                        continue;
                    else if (retention == NodeRetentionType.Collapse)
                        foreach (var sub in this.FilteredMembers(member.AsNode.Id, member.AsNode.Members))
                            yield return sub;
                    else if (retention == NodeRetentionType.CollapseSingle && member.AsNode.Members.Count == 1)
                        foreach (var sub in this.FilteredMembers(member.AsNode.Id, member.AsNode.Members))
                            yield return sub;
                    else if (retention == NodeRetentionType.Rollup && member.AsNode.Id == parentId)
                        foreach (var sub in this.FilteredMembers(parentId, member.AsNode.Members))
                            yield return sub;
                    else
                        yield return this.Collapsed(member.AsNode);
                }
                else
                    yield return member;
            }
        }

        private Lazy<String> LazySymbol(Identifier id)
            => this._SymbolIndex.ContainsKey(id)
               ? this._SymbolIndex[id]
               : Return.After(() => { this._SymbolIndex.Add(id, new Lazy<String>(() => this.GetSymbol(id))); },
                              () => this._SymbolIndex[id]);

        public Node CreateNode(Identifier id, IEnumerable<NodeOrToken> members)
            => new Node(id, this.LazySymbol(id), members);

        // Note: This means retention type on the start rule is ignored.
        private Node Collapsed(Node node)
            => this.CreateNode(node.Id, FilteredMembers(node.Id, node.Members));


        /// <summary>
        /// Parse returns either the root node of the abstract syntax tree upon success
        /// or the incomplete parse chart if the parse fails. The chart can be passed
        /// into the constructor of ParseError to get basic information about the error.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public ParseResult Parse(String source, Token[] tokens, Identifier newLineId)
        {
            var chart = tokens.Chart(this);

            if (chart[chart.Length - 1].Any(entry => entry.Rule.Symbol(this).DataEquals(this.StartSymbol) && entry.Number == 0))
            {
                var final = new FinalChart(chart);

                if (final[0].Any(entry => entry.Rule.Symbol(this).DataEquals(this.StartSymbol)))
                {
                    var ast = Node.Create(final, tokens);

                    if (ast is Some<Node> some)
                        return new ParseTree(this.Collapsed(some.Value), final);
                }
            }

            return new ParseError(chart, source, null, newLineId, tokens);
        }
    }

}
