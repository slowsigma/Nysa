using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

public static class Take
{
    static Take() { Take.IgnoreCase = true; }
    public static Boolean IgnoreCase { get; set; }


    private static Rule[] LongestAlternates(IEnumerable<Rule> alternates)
    {
        var actuals = alternates.SomeOnly();

        // flatten alternatives that are themselves LongestRule
        return actuals.Where(a => !(a is LongestRule))
                      .Concat(actuals.Where(a => a is LongestRule)
                                     .SelectMany(l => ((LongestRule)l).Alternatives))
                      .ToArray();
    }

    public static LongestRule Longest(IEnumerable<Rule> alternatives, params Rule[] additional)
    {
        var rules = LongestAlternates(alternatives.Concat(additional));

        return (rules.Length > 1)
               ? new LongestRule(rules)
               : throw new Exception("LongestRule requires two or more alternatives.");
    }

    public static LongestRule Longest(params Rule[] alternatives)
    {
        var rules = LongestAlternates(alternatives);

        return (rules.Length > 1)
               ? new LongestRule(rules)
               : throw new Exception("LongestRule requires two or more alternatives.");
    }

    public static SetRule Set(this String alternatives, Boolean? ignoreCase = null)
    {
        var set = ignoreCase.GetValueOrDefault(Take.IgnoreCase)
                  ? alternatives.ToArray().Select(c => Char.ToUpperInvariant(c)).ToHashSet()
                  : alternatives.ToArray().ToHashSet();

        return (set.Count > 1)
               ? new SetRule(set, ignoreCase.GetValueOrDefault(Take.IgnoreCase))
               : throw new Exception("SetRule requires two or more values.");
    }

    public static ThenRule Then(this Rule first, Rule next) => new ThenRule(first, next);
    public static ThenRule Then(this Rule first, Identifier id) => new ThenRule(first, new IdRule(id));

    public static OrRule Or(this Rule primary, Rule secondary) => new OrRule(primary, secondary);
    public static OneRule One(this Char value, Boolean? ignoreCase = null) => new OneRule(value, ignoreCase.GetValueOrDefault(Take.IgnoreCase));
    public static SequenceRule Sequence(this String value, Boolean? ignoreCase = null) => new SequenceRule(value, ignoreCase.GetValueOrDefault(Take.IgnoreCase));
    public static IdRule IdX(this Identifier id) => new IdRule(id);
    public static AndRule And(this Rule primary, Rule secondary) => new AndRule(primary, secondary);
    public static NotRule Not(this Rule condition) => new NotRule(condition);
    public static MaybeRule Maybe(this Rule condition) => new MaybeRule(condition);
    public static UntilRule Until(Rule condition) => new UntilRule(condition);
    public static WhileRule While(this Rule condition) => new WhileRule(condition);
    public static StackRule Stack(this Rule push, Rule pop) => new StackRule(push, pop);

    public static EqualRule Equal(this Rule subject, Rule check) => new EqualRule(subject, check);
    public static NotEqualRule NotEqual(this Rule subject, Rule check) => new NotEqualRule(subject, check);

    public static AssertRule Where(Func<TextSpan, Boolean> predicate)
        => new AssertRule(predicate);
    public static Rule Where(this Rule primary, Func<TextSpan, Boolean> predicate)
        => new ThenRule(primary, new AssertRule(predicate));
    
    public static SeekRule Seek(this Rule subject) => new SeekRule(subject);

    public static Rule Literals(IEnumerable<(String Sequence, Identifier Id)> literals, Boolean? ignoreCase = null)
    {
        var si        = ignoreCase.GetValueOrDefault(Take.IgnoreCase);
        var charNodes = new Dictionary<Char, CharacterNode>();

        foreach (var literal in literals.Where(t => !String.IsNullOrWhiteSpace(t.Sequence)))
        {
            var first = si
                        ? Char.ToUpperInvariant(literal.Sequence[0])
                        : literal.Sequence[0];

            if (!charNodes.ContainsKey(first))
                charNodes.Add(first, new CharacterNode(literal.Sequence, literal.Id, si));
            else
                charNodes[first].Add(literal.Sequence, literal.Id);
        }

        var nextRules   = charNodes.Select(n => n.Value.ToRule()).ToArray();
        // start with everything that we're not turning into a set
        var nexts       = nextRules.Where(n => !(n is OneRule o && o.IgnoreCase == si)).ToList();
        // pull out ones we can make into a set
        var oneRules    = nextRules.Where(n => n is OneRule o && o.IgnoreCase == si).Cast<OneRule>().ToArray();
        // if have enough for a set
        if (oneRules.Length > 1) // create it and add to nexts
            nexts.Add(oneRules.Aggregate(String.Empty, (s, o) => String.Concat(s, o.Value)).Set(si));
        else // otherwise, put what we can't use back into nexts
            nexts.AddRange(oneRules);
        // if we have more than one, we turn nexts into a single LongestRule
        if (nexts.Count > 1)
            nexts = Return.Enumerable(Take.Longest(nexts)).ToList<Rule>();

        return nexts.Count == 1
               ? nexts[0]
               : throw new Exception("Must have one or more literal values to make a rule.");
    }

    public static readonly AssertRule AtStart = new AssertRule(c => c.Start.IsSourceStart());
    public static readonly AssertRule AtEnd   = new AssertRule(c => c.End.IsSourceEnd());

    public static IEnumerable<LexHit> Repeat(this SeekRule seek, String source, Boolean includeSkips = false)
    {
        var curr = Start.Span(source);
        var find = seek.Function(curr);
        var diff = find.Match(h => h.Span.Position - curr.End.Value, m => 0);

        while (find is LexHit findHit)
        {
            if (diff > 0 && includeSkips)
                yield return Lex.Hit(new TextSpan(source, curr.End.Value, diff));

            yield return findHit;

            curr = findHit.Span;
            find = seek.Function(Start.SpanAfter(curr));
            diff = find.Match(h => h.Span.Position - curr.End.Value, m => 0);
        }
    }


}