using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {
        // static members
        public static readonly String InvalidSymbol = "invalid-symbol-id";

        public static Boolean IsLiteralSymbol (String symbol) => (!IsRuleSymbol(symbol) && !IsCategorySymbol(symbol));
        public static Boolean IsRuleSymbol    (String symbol) => Factory.StrictRuleSymbol.Find(symbol.Start()).IsHit;
        public static Boolean IsCategorySymbol(String symbol) => Factory.StrictCategorySymbol.Find(symbol.Start()).IsHit;

        // We use a null list of rule variants to signify terminal symbols.
        private static readonly List<RuleVariant> Terminal = null;

        private static Suspect<Grammar> Create(Factory factory)
        {
            // Note: The factory.Symbols() function creates a list of every rule and category
            //       symbol as well as every literal string. We "symbolize" literals to
            //       force lexical analysis out of the parser (we parse tokens not characters).
            var idIndex     = factory.Symbols()
                                     .Select((s, i) => (Symbol: s, Id: Identifier.FromInteger(i + 1)))
                                     .ToDictionary(k => k.Symbol,
                                                   v => v.Id,
                                                   StringComparer.OrdinalIgnoreCase);

            Identifier Find(String symbol)
                => idIndex.Lookup(symbol, Identifier.None);

            var definitions = factory.Rules()
                                     .ToDictionary(k => k.Symbol, 
                                                   v => new Rule(v.Symbol,
                                                                 v.Definitions.Select(d => new RuleVariant(Find(v.Symbol), d.Select(e => Find(e.Value)))).ToList(),
                                                                 v.NodeRetention),
                                                   StringComparer.OrdinalIgnoreCase);

            // Assemble the rules and make special terminal rules for literals and categories.
            var rules       = idIndex.Select(kvp => definitions.ContainsKey(kvp.Key)
                                                    ? definitions[kvp.Key]
                                                    : new Rule(kvp.Key, Grammar.Terminal, NodeRetentionType.Keep))
                                     .ToDictionary(k => idIndex[k.Symbol]);

            // validate the start symbol
            if (!idIndex.ContainsKey(factory.StartSymbol))
                return (new Exception($"Error in grammar. The start symbol {factory.StartSymbol} is not defined.")).Failed<Grammar>();

            // find undefined rule symbols
            var undefined = idIndex.Select(kvp => kvp.Key)
                                   .Where(s => Grammar.IsRuleSymbol(s))
                                   .Where(r => !rules.ContainsKey(idIndex[r]))
                                   .ToList();

            if (undefined.Count > 0)
                return (new Exception($"Error in grammar. The following symbols are not defined: {String.Join(" ", undefined)}")).Failed<Grammar>();

            // non-terminal rule variants
            var properRules = rules.Where(r => !r.Value.IsTerminal)
                                   .SelectMany(kvp => kvp.Value.Variants)
                                   .ToList();

            // terminal identifiers
            var terminalIds = new HashSet<Identifier>(rules.Where(r => r.Value.IsTerminal)
                                                           .Select(t => t.Key));

            // non-terminal rule variants that ...
            // reference a terminal
            var terminals   = new HashSet<RuleVariant>(properRules.Where(v => v.DefinitionIds.Any(i => terminalIds.Contains(i))));
            // establish the symbol as nullable
            var nullables   = new HashSet<RuleVariant>(properRules.Where(v => v.IsEmpty));
            // are not in terminals and not in nullables
            var unknowns    = new HashSet<RuleVariant>(properRules.Where(v => !terminals.Contains(v) && !nullables.Contains(v)));

            var nullableIds = new HashSet<Identifier>(nullables.Select(v => v.Id)
                                                               .Distinct());

            var moreNulls   = unknowns.Where(v => v.DefinitionIds.All(s => nullableIds.Contains(s))).ToArray();

            while (moreNulls.Length > 0)
            {
                nullables.UnionWith(moreNulls);
                unknowns.ExceptWith(moreNulls);

                nullableIds.UnionWith(moreNulls.Select(n => n.Id).Distinct());
                moreNulls = unknowns.Where(v => v.DefinitionIds.All(s => nullableIds.Contains(s))).ToArray();
            }

            // At this point there may be variants left over in the unknowns collection
            // and we would dump them into the terminals collection but those two are
            // not used past this point.

            // Start of analysis to validate there are no infinite loops from nullable rules back to themselves
            IEnumerable<Identifier> NonEmptyChildren(Identifier nullableId)
            {
                return nullables.Where(v => v.Id == nullableId &&
                                            !v.IsEmpty)
                                .SelectMany(r => r.DefinitionIds);
            }

            Option<List<Identifier>> InfinitePath(Identifier startingId, Identifier currentId, List<Identifier> path)
            {
                path.Add(currentId);

                var first = NonEmptyChildren(currentId).Select(nec => nec == startingId
                                                                      ? path.Some()
                                                                      : InfinitePath(startingId, nec, path))
                                                       .FirstSome();
                                                       

                if (first is Some<List<Identifier>> && startingId == currentId)
                    path.Add(startingId);

                return first;
            }

            var infiniteNull = nullableIds.Select(n => InfinitePath(n, n, new List<Identifier>()))
                                          .FirstSome();

            return infiniteNull.Match(infinitePath => (new Exception($"Error in grammar. Nullable rules lead to the infinite path: {String.Join(" -> ", infinitePath.Select(p => rules[p].Symbol))}")).Failed<Grammar>(),
                                      () => (new Grammar(factory.StartSymbol, idIndex, rules, nullableIds)).Confirmed());
        }

    }

}
