using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;

namespace Dorata.Text.Lexing
{

    public static partial class Take
    {

        public static readonly Miss EMTPY_MISS = new Miss(0);

        public struct Hit
        {
            public TextSpan   Span  { get; private set; }
            public Identifier Id    { get; private set; }
            public Hit(TextSpan span, Identifier id) { this.Span = span; this.Id = id; }
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
                => first.OrValue(EMTPY_MISS).Size > second.OrValue(EMTPY_MISS).Size ? first.OrValue(EMTPY_MISS) : second.OrValue(EMTPY_MISS);
            public static Miss Smallest(Option<Miss> first, Option<Miss> second)
                => first.OrValue(EMTPY_MISS).Size < second.OrValue(EMTPY_MISS).Size ? first.OrValue(EMTPY_MISS) : second.OrValue(EMTPY_MISS);

            public static Option<Hit> Largest(Option<Hit> first, Option<Hit> second)
            {
                var firstLength  = first.Select(f => f.Span.Length).OrValue(-1);
                var secondLength = second.Select(s => s.Span.Length).OrValue(-1);

                return   firstLength  > secondLength ? first.Value
                       : secondLength > -1           ? second.Value
                       :                               Option<Hit>.None;
            }

            public static Option<Hit> Identified(Option<Hit> first, Option<Hit> second)
            {
                var firstId  = first.Select(f => f.Id).OrValue(Identifier.None);
                var secondId = second.Select(s => s.Id).OrValue(Identifier.None);

                return   firstId  != Identifier.None ? first.Value
                       : secondId != Identifier.None ? second.Value
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
            public Option<Hit> ToHit() { return this.IsHit ? this.Hit : Option<Hit>.None; }
            public Option<Miss> ToMiss() { return this.IsHit ? this.Miss : Option<Miss>.None; }

        }

    } // Take

}
