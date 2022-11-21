using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

public abstract record Rule(Func<TextSpan, LexFind> Function);

public sealed record AssertRule(Func<TextSpan, Boolean> Predicate) : Rule(c => Predicate(c) ? Lex.Hit(c) : Lex.Miss(0));

public sealed record IdRule(Identifier Id) : Rule(c => Lex.Hit(c, Id.ToTokenIdentifier()));

public sealed record OneRule(
    Char Value,
    Boolean IgnoreCase
) : Rule(c => RuleFunctions.FindOne(Value, IgnoreCase, c));

public sealed record SetRule(
    IReadOnlySet<Char> Values,      // Build with Char.ToUpperInvariant when this.IgnoreCase
    Boolean IgnoreCase
) : Rule(c => RuleFunctions.FindSet(Values, IgnoreCase, c));

public sealed record SequenceRule(
    String Value,
    Boolean IgnoreCase
) : Rule(c => RuleFunctions.FindSequence(Value, IgnoreCase, c));

public sealed record OrRule(
    Rule Primary,
    Rule Secondary
) : Rule(c => RuleFunctions.FindOr(Primary, Secondary, c));

public sealed record ThenRule(
    Rule First,
    Rule Next
) : Rule(c => RuleFunctions.FindThen(First, Next, c));

public sealed record NotRule(
    Rule Condition
) : Rule(c => RuleFunctions.FindNot(Condition, c));

public sealed record AndRule(
    Rule One,
    Rule Two
) : Rule(c => RuleFunctions.FindAnd(One, Two, c));

public sealed record UntilRule(
    Rule Condition
) : Rule(c => RuleFunctions.FindUntil(Condition, c));

public sealed record WhileRule(
    Rule Condition
) : Rule(c => RuleFunctions.FindWhile(Condition, c));

public sealed record MaybeRule(
    Rule Condition
) : Rule(c => RuleFunctions.FindMaybe(Condition, c));

public sealed record LongestRule(
    IReadOnlyList<Rule> Alternatives
) : Rule(c => RuleFunctions.FindLongest(Alternatives, c));

public sealed record EqualRule(
    Rule Subject,
    Rule Check
) : Rule(c => RuleFunctions.FindEqual(Subject, Check, c));

public sealed record NotEqualRule(
    Rule Subject,
    Rule Check
) : Rule(c => RuleFunctions.FindNotEqual(Subject, Check, c));

public sealed record StackRule(
    Rule Push,
    Rule Pop
) : Rule(c => RuleFunctions.FindStack(Push, Pop, c));

public sealed record SeekRule(
    Rule Subject
) : Rule(c => RuleFunctions.FindSeek(Subject, c));
