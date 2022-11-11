using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

public static class Find
{
    static Find() { Find.IgnoreCase = true; }
    public static Boolean IgnoreCase { get; set; }


    private static Rule[] LongestAlternates(IEnumerable<Option<Rule>> alternates)
    {
        var actuals = alternates.SomeOnly();

        // flatten alternatives that are themselves LongestRule
        return actuals.Where(a => !(a is LongestRule))
                      .Concat(actuals.Where(a => a is LongestRule)
                                     .SelectMany(l => ((LongestRule)l).Alternatives))
                      .ToArray();
    }

    public static Option<LongestRule> Longest(IEnumerable<Option<Rule>> alternatives, params Option<Rule>[] additional)
    {
        var rules = LongestAlternates(alternatives.Concat(additional));

        return (rules.Length > 1) ? (new LongestRule(rules)).Some() : Option.None;
    }

    public static Option<LongestRule> Longest(params Option<Rule>[] alternatives)
    {
        var rules = LongestAlternates(alternatives);

        return (rules.Length > 1) ? (new LongestRule(rules)).Some() : Option.None;
    }

    public static Option<SetRule> Set(this String alternatives, Boolean? ignoreCase = null)
    {
        var set = ignoreCase.GetValueOrDefault(Find.IgnoreCase)
                  ? alternatives.ToArray().Select(c => Char.ToUpperInvariant(c)).ToHashSet()
                  : alternatives.ToArray().ToHashSet();

        return (set.Count > 1)
               ? (new SetRule(set, ignoreCase.GetValueOrDefault(Take.IgnoreCase))).Some()
               : Option.None;
    }

    public static ThenRule Then(this Rule first, Rule next) => new ThenRule(first, next);
    public static ThenRule Then(this Rule first, Identifier id) => new ThenRule(first, new IdRule(id));

    public static OrRule Or(this Rule primary, Rule secondary) => new OrRule(primary, secondary);
    public static OneRule One(this Char value, Boolean? ignoreCase = null) => new OneRule(value, ignoreCase.GetValueOrDefault(Take.IgnoreCase));
    public static SequenceRule Sequence(this String value, Boolean? ignoreCase = null) => new SequenceRule(value, ignoreCase.GetValueOrDefault(Take.IgnoreCase));
    public static IdRule Id(this Identifier id) => new IdRule(id);
    public static AndRule And(this Rule primary, Rule secondary) => new AndRule(primary, secondary);
    public static NotRule Not(this Rule condition) => new NotRule(condition);
    public static MaybeRule Maybe(this Rule condition) => new MaybeRule(condition);
    public static UntilRule Until(Rule condition) => new UntilRule(condition);
    public static WhileRule While(this Rule condition) => new WhileRule(condition);
    public static StackRule Stack(this Rule push, Rule pop) => new StackRule(push, pop);

    public static AssertRule Where(Func<TextSpan, Boolean> predicate)
        => new AssertRule(predicate);
    public static Rule Where(this Rule primary, Func<TextSpan, Boolean> predicate)
        => new ThenRule(primary, new AssertRule(predicate));
    
    public static SeekRule Seek(this Rule subject) => new SeekRule(subject);

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