using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Lexing
{

    public static partial class Take
    {

        public static readonly Miss EMTPY_MISS = new Miss(0);

        public struct Hit
        {
            public TextSpan   Span  { get; private set; }
            public Identifier Id    { get; private set; }
            public Hit(TextSpan span, Identifier id) { this.Span = span; this.Id = id; }
            public override String ToString() => $"{this.Id} '{this.Span}'";
        }

        public struct Miss
        {
            public Int32 Size { get; private set; }
            public Miss(Int32 size) { this.Size = size; }
        }

        public class HitOrMiss
        {
            public static implicit operator HitOrMiss(Hit  hit ) => new HitOrMiss(hit );
            public static implicit operator HitOrMiss(Miss miss) => new HitOrMiss(miss);

            public static HitOrMiss NewHit(TextSpan span, Identifier? id = null) => new Hit(span, id ?? Identifier.None);
            public static HitOrMiss NewMiss(Int32 size) => new Miss(size);

            public static Miss Largest(Option<Miss> first, Option<Miss> second)
                => first.Or(EMTPY_MISS).Size > second.Or(EMTPY_MISS).Size ? first.Or(EMTPY_MISS) : second.Or(EMTPY_MISS);
            public static Miss Smallest(Option<Miss> first, Option<Miss> second)
                => first.Or(EMTPY_MISS).Size < second.Or(EMTPY_MISS).Size ? first.Or(EMTPY_MISS) : second.Or(EMTPY_MISS);

            public static Option<Hit> Largest(Option<Hit> first, Option<Hit> second)
            {
                var firstLength  = first.Map(f => f.Span.Length).Or(-1);
                var secondLength = second.Map(s => s.Span.Length).Or(-1);

                return   firstLength  > secondLength ? first
                       : secondLength > -1           ? second
                       :                               Option<Hit>.None;
            }

            public static Option<Hit> Identified(Option<Hit> first, Option<Hit> second)
            {
                var firstId  = first.Map(f => f.Id).Or(Identifier.None);
                var secondId = second.Map(s => s.Id).Or(Identifier.None);

                return   firstId  != Identifier.None ? first
                       : secondId != Identifier.None ? second
                       :                               Option<Hit>.None;
            }

            // instance members
            public Hit      Hit     { get; private set; }
            public Miss     Miss    { get; private set; }
            public Boolean  IsHit   { get; private set; }
            public Boolean  IsMiss  { get => !this.IsHit; }

            public HitOrMiss(Hit hit) { this.IsHit = true; this.Hit = hit; }
            public HitOrMiss(Miss miss) { this.IsHit = false; this.Miss = miss; }
            public T Select<T>(Func<Hit, T> fA, Func<Miss, T> fB) { return this.IsHit ? fA(this.Hit) : fB(this.Miss); }
            public void Apply(Action<Hit> actHit, Action<Miss> actMiss) { if (this.IsHit) actHit(this.Hit); else actMiss(this.Miss); }
            public Option<Hit> ToHit() { return this.IsHit ? this.Hit.Some() : Option<Hit>.None; }
            public Option<Miss> ToMiss() { return this.IsHit ? this.Miss.Some() : Option<Miss>.None; }

        }

    } // Take

}
