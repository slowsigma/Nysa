using System;

using Nysa.Logics;

namespace Nysa.Text.Lexing
{

    public abstract record LexFind
    {
        public static readonly LexMiss EMTPY_MISS = new LexMiss(0);

        public static LexFind Hit(TextSpan span, Identifier? id = null) => new LexHit(span, id ?? Identifier.None);
        public static LexFind Miss(Int32 size) => new LexMiss(size);


        public static LexMiss Largest(Option<LexMiss> first, Option<LexMiss> second)
            => first.Or(EMTPY_MISS).Size > second.Or(EMTPY_MISS).Size ? first.Or(EMTPY_MISS) : second.Or(EMTPY_MISS);
        public static LexMiss Smallest(LexMiss first, LexMiss second)
            => first.Size < second.Size ? first : second;

        public static LexHit Largest(LexHit first, LexHit second)
        {
            var firstLength  = first.Span.Length;
            var secondLength = second.Span.Length;

            return (firstLength > secondLength) ? first : second;
        }

        public static LexHit Identified(LexHit first, LexHit second)
        {
            var firstId  = first.Id;
            var secondId = second.Id;

            return (firstId != Identifier.None) ? first : second;
        }

        public T Map<T>(Func<LexHit, T> fA, Func<LexMiss, T> fB)
        {
            return   this is LexHit hit ? fA(hit)
                   : this is LexMiss miss ? fB(miss)
                   : throw new Exception("Unexpected type.");
        }

        public Unit Affect(Action<LexHit> actHit, Action<LexMiss> actMiss)
        {
            if (this is LexHit hit)
                actHit(hit);
            else if (this is LexMiss miss)
                actMiss(miss);
            else
                throw new Exception("Unexpected type.");

            return Unit.Value;
        }

    }

    public record LexHit : LexFind
    {
        public TextSpan   Span  { get; private set; }
        public Identifier Id    { get; private set; }
        public LexHit(TextSpan span, Identifier id) { this.Span = span; this.Id = id; }
    }

    public record LexMiss : LexFind
    {
        public Int32 Size { get; private set; }
        public LexMiss(Int32 size) { this.Size = size; }
    }

}