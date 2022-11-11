using System;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

public static class Lex
{
    public static readonly LexMiss EMTPY_MISS = new LexMiss(0);
    public static readonly LexMiss SINGLE_MISS = new LexMiss(1);

    public static LexHit  Hit(TextSpan span, Identifier? id = null) => new LexHit(span, id ?? Identifier.None);
    public static LexMiss Miss(Int32 size) => new LexMiss(size);

    public static LexMiss Largest(Option<LexMiss> first, Option<LexMiss> second)
        => first.Or(EMTPY_MISS).Size > second.Or(EMTPY_MISS).Size ? first.Or(EMTPY_MISS) : second.Or(EMTPY_MISS);
    public static LexMiss Smallest(LexMiss first, LexMiss second)
        => first.Size < second.Size ? first : second;

    public static LexHit Largest(LexHit first, LexHit second)
        => (first.Span.Length > second.Span.Length) ? first : second;
    

    public static LexHit Identified(LexHit first, LexHit second)
        => (first.Id != Identifier.None) ? first : second;

    public static T Match<T>(this LexFind @this, Func<LexHit, T> fA, Func<LexMiss, T> fB)
        =>   @this is LexHit  hit  ? fA(hit)
           : @this is LexMiss miss ? fB(miss)
           : throw new Exception("Unexpected type.");

    public static Unit Affect(this LexFind @this, Action<LexHit> whenHit, Action<LexMiss> whenMiss)
    {
        if (@this is LexHit hit)
            whenHit(hit);
        else if (@this is LexMiss miss)
            whenMiss(miss);
        else
            throw new Exception("Unexpected type.");

        return Unit.Value;
    }

}