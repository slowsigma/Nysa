using System;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

public abstract record LexFind();

public sealed record LexHit(
    TextSpan        Span,
    TokenIdentifier Id
) : LexFind();

public sealed record LexMiss(
    Int32 Size
) : LexFind();
