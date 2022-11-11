using System;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

public abstract record LexFind();

public record LexHit(
    TextSpan   Span,
    Identifier Id
) : LexFind();

public record LexMiss(
    Int32 Size
) : LexFind();
