using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

public abstract record Rule(Func<TextSpan, LexFind> Function);

public abstract record AssertRule(Func<TextSpan, Boolean> Predicate) : Rule(c => Predicate(c) ? Lex.Hit(c) : Lex.Miss(0));

public sealed record StartRule() : AssertRule(current => current.Start.IsSourceStart());

public sealed record EndRule() : AssertRule(current => current.End.IsSourceEnd());

public record IdRule(Identifier Id) : Rule(c => Lex.Hit(c, Id));

public record OneRule(
    Char Value,
    Boolean IgnoreCase
) : Rule(c => RuleFunctions.FindOne(Value, IgnoreCase, c));

public record SetRule(
    IReadOnlySet<Char> Values,      // Build with Char.ToUpperInvariant when this.IgnoreCase
    Boolean IgnoreCase
) : Rule(c => RuleFunctions.FindSet(Values, IgnoreCase, c));

public record SequenceRule(
    String Value,
    Boolean IgnoreCase
) : Rule(c => RuleFunctions.FindSequence(Value, IgnoreCase, c));

public record OrRule(
    Rule Primary,
    Rule Secondary
) : Rule(c => RuleFunctions.FindOr(Primary, Secondary, c));

public record ThenRule(
    Rule First,
    Rule Next
) : Rule(c => RuleFunctions.FindThen(First, Next, c));

public record NotRule(
    Rule Condition
) : Rule(c => RuleFunctions.FindNot(Condition, c));

public record AndRule(
    Rule One,
    Rule Two
) : Rule(c => RuleFunctions.FindAnd(One, Two, c));

public record UntilRule(
    Rule Condition
) : Rule(c => RuleFunctions.FindUntil(Condition, c));

public record WhileRule(
    Rule Condition
) : Rule(c => RuleFunctions.FindWhile(Condition, c));

public record MaybeRule(
    Rule Condition
) : Rule(c => RuleFunctions.FindMaybe(Condition, c));

public record LongestRule(
    IReadOnlyList<Rule> Alternatives
) : Rule(c => RuleFunctions.FindLongest(Alternatives, c));

public record StackRule(
    Rule Push,
    Rule Pop
) : Rule(c => RuleFunctions.FindStack(Push, Pop, c));
