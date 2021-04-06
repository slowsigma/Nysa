using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Logics
{

    public static class OptionExtensions
    {
        private static readonly String UsageErrorTemplate    = "{0} does not accept types other than Some<T> and None<T>.";
        private static readonly String MapUsageErrorString   = String.Format(UsageErrorTemplate, nameof(Map));
        private static readonly String BindUsageErrorString  = String.Format(UsageErrorTemplate, nameof(Bind));
        private static readonly String ApplyUsageErrorString = String.Format(UsageErrorTemplate, nameof(Apply));
        private static readonly String MatchUsageErrorString = String.Format(UsageErrorTemplate, nameof(Match));
        private static readonly String OrUsageErrorString = String.Format(UsageErrorTemplate, nameof(Or));

        public static Option<U> Map<T, U>(this Option<T> @this, Func<T, U> transform)
            =>   @this is Some<T> some ? transform(some.Value).Some()
               : @this is None<T>      ? Option<U>.None
               :                         throw new ArgumentException(MapUsageErrorString, nameof(@this));

        public static async Task<Option<U>> MapAsync<T, U>(this Option<T> @this, Func<T, Task<U>> transformAsync)
            =>   @this is Some<T> some ? (await transformAsync(some.Value)).Some()
               : @this is None<T>      ? Option<U>.None
               :                         throw new ArgumentException(MapUsageErrorString, nameof(@this));

        public static Option<Func<T2, TR>> Map<T1, T2, TR>(this Option<T1> @this, Func<T1, T2, TR> transform)
            => @this.Map(transform.Curry());

        public static Option<U> Apply<T, U>(this Option<Func<T, U>> @this, Option<T> argument)
            =>   @this is Some<Func<T, U>> sfa && argument is Some<T> saa ? (Some<U>)sfa.Value(saa.Value)
               : @this is None<Func<T, U>> nfb && argument is Some<T> sab ? Option<U>.None
               : @this is Some<Func<T, U>> sfc && argument is None<T> nac ? Option<U>.None
               : @this is None<Func<T, U>> nfd && argument is None<T> nad ? Option<U>.None
               :                                                            throw new ArgumentException(ApplyUsageErrorString);

        public static Option<Func<T2, TR>> Apply<T1, T2, TR>(this Option<Func<T1, T2, TR>> @this, Option<T1> argument)
            => Apply(@this.Map(Functional.Curry), argument);

        public static Option<U> Bind<T, U>(this Option<T> @this, Func<T, Option<U>> transform)
            =>   @this is Some<T> some ? transform(some.Value)
               : @this is None<T>      ? Option<U>.None
               :                         throw new ArgumentException(BindUsageErrorString, nameof(@this));

        public static R Match<T, R>(this Option<T> @this, Func<T, R> whenSome, R whenNone)
            =>   @this is Some<T> some ? whenSome(some.Value)
               : @this is None<T>      ? whenNone
               :                         throw new ArgumentException(MatchUsageErrorString, nameof(@this));

        public static R Match<T, R>(this Option<T> @this, Func<T, R> whenSome, Func<R> whenNone)
            =>   @this is Some<T> some ? whenSome(some.Value)
               : @this is None<T>      ? whenNone()
               :                         throw new ArgumentException(MatchUsageErrorString, nameof(@this));

        public static T Or<T>(this Option<T> @this, T whenNone)
            =>   @this is Some<T> some ? some.Value
               : @this is None<T>      ? whenNone
               :                         throw new ArgumentException(OrUsageErrorString, nameof(@this));

        public static T Or<T>(this Option<T> @this, Func<T> whenNone)
            =>   @this is Some<T> some ? some.Value
               : @this is None<T>      ? whenNone()
               :                         throw new ArgumentException(OrUsageErrorString, nameof(@this));

        public static Option<T> Or<T>(this Option<T> @this, Option<T> whenNone)
            =>   @this is Some<T> some ? @this
               : @this is None<T>      ? whenNone
               :                         throw new ArgumentException(OrUsageErrorString, nameof(@this));

        public static Option<T> Or<T>(this Option<T> @this, Func<Option<T>> whenNone)
            =>   @this is Some<T> some ? @this
               : @this is None<T>      ? whenNone()
               :                         throw new ArgumentException(OrUsageErrorString, nameof(@this));

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> @this, Func<T, Boolean> predicate = null)
        {
            predicate = predicate ?? (t => true);

            foreach (var item in @this)
                if (predicate(item))
                    return item.Some();

            return Option<T>.None;
        }

    }

}
