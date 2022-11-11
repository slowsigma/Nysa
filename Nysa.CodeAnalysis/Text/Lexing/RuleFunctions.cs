using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

public static class RuleFunctions
{
    public static LexFind Find(this IdRule @this, TextSpan current)
        => Lex.Hit(current, @this.Id);

    private static Boolean IsEqual(this Char @this, Char other, Boolean ignoreCase)
        =>    @this.Equals(other)
           || (ignoreCase && Char.ToUpperInvariant(@this).Equals(Char.ToUpperInvariant(other)));

    public static LexFind FindOne(Char value, Boolean ignoreCase, TextSpan current)
        => current.End
                  .Char()
                  .Map(n => n.IsEqual(value, ignoreCase) ? (LexFind)Lex.Hit(current.Appended(1)) : Lex.SINGLE_MISS)
                  .Or(Lex.SINGLE_MISS);

    public static LexFind FindSet(IReadOnlySet<Char> values, Boolean ignoreCase, TextSpan current)
        => current.End
                  .Char()
                  .Map(n => values.Contains(ignoreCase ? Char.ToUpperInvariant(n) : n) ? (LexFind)Lex.Hit(current.Appended(1)) : Lex.SINGLE_MISS)
                  .Or(Lex.SINGLE_MISS);

    public static LexFind FindSequence(String value, Boolean ignoreCase, TextSpan current)
    {
        var seqPos = 0;
        var curPos = current.End;

        while (curPos.Char() is Some<Char> someChar && seqPos < value.Length)
        {
            if (!value[seqPos].IsEqual(someChar.Value, ignoreCase))
                return Lex.Miss(value.Length);

            seqPos++;
            curPos = curPos + 1;
        }

        return seqPos == value.Length
               ? Lex.Hit(current.Appended(value.Length))
               : Lex.Miss(value.Length);
    }

    public static LexFind FindOr(Rule primary, Rule secondary, TextSpan current)
        => primary.Function(current)
                  .Match(h => h,
                         m => secondary.Function(current));

    // NOTE: If first rule gets a hit and next does not, the total miss is only the length added by first
    //       hit (i.e., first hit length - current length) plus the miss length of next.
    public static LexFind FindThen(Rule first, Rule next, TextSpan current)
        => first.Function(current)
                .Match(htOne => next.Function(htOne.Span)
                                    .Match(htTwo => (LexFind)htTwo,
                                           msTwo => (LexFind)Lex.Miss(msTwo.Size + (htOne.Span.Length - current.Length))), // htOne.Span.Length has original length
                                                                                                                           // plus whatever first rule added to it
                       msOne => msOne);

    public static LexFind FindNot(Rule condition, TextSpan current)
        => condition.Function(current)
                    .Match(h => (LexFind)Lex.Miss(h.Span.Length - current.Length),   // the NotRule miss is only the amount condition hit added to current
                           m => (LexFind)Lex.Hit(current.Appended(m.Size)));

    public static LexFind FindAnd(Rule one, Rule two, TextSpan current)
    {
        var r1 = one.Function(current).Make(r => (Hit: r as LexHit, Miss: r as LexMiss));
        var r2 = two.Function(current).Make(r => (Hit: r as LexHit, Miss: r as LexMiss));

        if (r1.Hit != null && r2.Hit != null)
        {
            var bigHit = Lex.Largest(r1.Hit, r2.Hit);
            var idHit  = Lex.Identified(r1.Hit, r2.Hit);

            return Lex.Hit(bigHit.Span, idHit.Id);
        }
        else if (r1.Hit != null && r2.Miss != null)
            return Lex.Smallest(Lex.Miss(r1.Hit.Span.Length - current.Length), r2.Miss);
        else if (r1.Miss != null && r2.Hit != null)
            return Lex.Smallest(r1.Miss, Lex.Miss(r2.Hit.Span.Length - current.Length));
        else if (r1.Miss != null && r2.Miss != null)
            return Lex.Smallest(r1.Miss, r2.Miss);
        else
            throw new Exception("Program error.");
    }

    public static LexFind FindUntil(Rule condition, TextSpan current)
    {
        var until       = condition.Function(current);
        var totalMiss   = 0;

        while (until is LexMiss untilMiss && !current.End.IsSourceEnd())
        {
            current   = current.Appended(untilMiss.Size);
            totalMiss += untilMiss.Size;

            until = condition.Function(current);
        }

        return until.Match(h => (LexFind)Lex.Hit(current),
                           m => (LexFind)Lex.Miss(totalMiss + m.Size));
    }

    public static LexFind FindWhile(Rule condition, TextSpan current)
    {
        var id = Identifier.None;

        while (!current.End.IsSourceEnd())
        {
            var find = condition.Function(current);

            if (find is LexMiss)
                return Lex.Hit(current);
            else if (find is LexHit hit)
            {
                current = hit.Span;
                id      = hit.Id;
            }
            else
                throw new Exception("Unexpected type.");
        }

        return Lex.Hit(current, id);
    }

    public static LexFind FindMaybe(Rule condition, TextSpan current)
        => condition.Function(current).Match(h => h, m => Lex.Hit(current));

    public static LexFind FindLongest(IReadOnlyList<Rule> alternatives, TextSpan current)
    {
        var miss    = Lex.Miss(Int32.MaxValue);
        var hit     = Lex.Hit(current);
        var isHit   = false;

        foreach (var rule in alternatives)
        {
            var find = rule.Function(current);

            isHit |= find is LexHit;

            find.Affect(h => { hit  = hit.Span.Length < h.Span.Length ? h : hit; },
                        m => { miss = m.Size          < miss.Size     ? m : miss; });
        }

        return isHit
               ? hit
               : (miss.Size == Int32.MaxValue ? Lex.Miss(0) : miss);
    }

    private static LexFind SpanPush(LexHit hit, Rule push, Rule pop)
    {
        var popped      = pop.Function(hit.Span);       // give pop a try first
        var @continue   = !hit.Span.End.IsSourceEnd();  // even at the end of the source

        while (popped is LexMiss && @continue)
        {
            // need to check push before expanding hit
            var pushed = push.Function(hit.Span);

            if (pushed is LexHit pushHit)
                SpanPush(pushHit, push, pop).Affect(h => { hit = h; }, m => { @continue = false; });
            else // no new push for now, expand hit by one
                hit = Lex.Hit(hit.Span.Appended(1));

            popped      = pop.Function(hit.Span);
            @continue   = !hit.Span.End.IsSourceEnd();
        }

        return popped;
    }

    public static LexFind FindStack(Rule push, Rule pop, TextSpan current)
    {
        var start = push.Function(current);

// NOTE: Prior code create a new Miss with Size = 1 instead of using the miss size of the push rule.
//       We need to find out why???
        return start.Match(h => SpanPush(h, push, pop),
                           m => m);
    }

}