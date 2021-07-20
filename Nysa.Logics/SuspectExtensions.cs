using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Logics
{

    public static class SuspectExtensions
    {
        private static readonly String UsageErrorTemplate           = "{0} does not accept types other than Confirmed<T> and Failed<T>.";
        private static readonly String MapUsageErrorString          = String.Format(UsageErrorTemplate, nameof(Map));
        private static readonly String MapAsyncUsageErrorString     = String.Format(UsageErrorTemplate, nameof(MapAsync));
        private static readonly String BindUsageErrorString         = String.Format(UsageErrorTemplate, nameof(Bind));
        private static readonly String BindAsyncUsageErrorString    = String.Format(UsageErrorTemplate, nameof(BindAsync));
        private static readonly String ApplyUsageErrorString        = String.Format(UsageErrorTemplate, nameof(Apply));
        private static readonly String ApplyAsyncUsageErrorString   = String.Format(UsageErrorTemplate, nameof(ApplyAsync));
        private static readonly String MatchUsageErrorString        = String.Format(UsageErrorTemplate, nameof(Match));
        private static readonly String MatchAsyncUsageErrorString   = String.Format(UsageErrorTemplate, nameof(MatchAsync));
        private static readonly String AffectUsageErrorString       = String.Format(UsageErrorTemplate, nameof(Affect));
        private static readonly String AffectAsyncUsageErrorString  = String.Format(UsageErrorTemplate, nameof(AffectAsync));

        public static Suspect<R> Map<T, R>(this Suspect<T> @this, Func<T, R> transform)
            =>   (@this is Confirmed<T> confirmed) ? transform(confirmed.Value).Confirmed()
               : (@this is Failed<T>    failed)    ? (Failed<R>)failed.Value
               :                                     throw new ArgumentException(MapUsageErrorString, nameof(@this));

        public static Suspect<Func<B, R>> Map<A, B, R>(this Suspect<A> @this, Func<A, B, R> transform)
            => @this.Map(transform.Curry());

        public static async Task<Suspect<R>> MapAsync<T, R>(this Suspect<T> @this, Func<T, Task<R>> transformAsync)
            =>   (@this is Confirmed<T> confirmed) ? (await transformAsync(confirmed.Value)).Confirmed()
               : (@this is Failed<T> failed)       ? (Failed<R>)failed.Value
               :                                     throw new ArgumentException(MapAsyncUsageErrorString, nameof(@this));

        public static Suspect<Func<B, Task<R>>> Map<A, B, R>(this Suspect<A> @this, Func<A, B, Task<R>> transformAsync)
            => @this.Map(transformAsync.Curry());

        public static Suspect<R> Bind<T, R>(this Suspect<T> @this, Func<T, Suspect<R>> transform)
            =>   (@this is Confirmed<T> confirmed) ? transform(confirmed.Value)
               : (@this is Failed<T> failed)       ? (Failed<R>)failed.Value
               :                                     throw new ArgumentException(BindUsageErrorString, nameof(@this));

        public static async Task<Suspect<R>> BindAsync<T, R>(this Suspect<T> @this, Func<T, Task<Suspect<R>>> transformAsync)
            =>   (@this is Confirmed<T> confirmed) ? (await transformAsync(confirmed.Value))
               : (@this is Failed<T> failed)       ? (Failed<R>)failed.Value
               :                                     throw new ArgumentException(BindAsyncUsageErrorString, nameof(@this));

        private static IEnumerable<Exception> Flat(this Exception @this)
            => @this is AggregateException thisAgg ? thisAgg.InnerExceptions : Return.Enumerable(@this);

        public static Suspect<R> Apply<T, R>(this Suspect<Func<T, R>> @this, Suspect<T> given)
            =>   @this is Confirmed<Func<T, R>> cfa && given is Confirmed<T> cga ? cfa.Value(cga.Value).Confirmed()
               : @this is Failed<Func<T, R>>    ffb && given is Confirmed<T> cgb ? ffb.Value.Failed<R>()
               : @this is Confirmed<Func<T, R>> cfc && given is Failed<T>    fgc ? fgc.Value.Failed<R>()
               : @this is Failed<Func<T, R>>    ffd && given is Failed<T>    fgd ? (new AggregateException(ffd.Value.Flat().Concat(fgd.Value.Flat()))).Failed<R>()
               :                                                                   throw new ArgumentException(ApplyUsageErrorString);

        public static async Task<Suspect<R>> ApplyAsync<T, R>(this Suspect<Func<T, Task<R>>> @this, Suspect<T> given)
            =>   @this is Confirmed<Func<T, Task<R>>> cfa && given is Confirmed<T> cga ? (await cfa.Value(cga.Value)).Confirmed()
               : @this is Failed<Func<T, Task<R>>>    ffb && given is Confirmed<T> cgb ? ffb.Value.Failed<R>()
               : @this is Confirmed<Func<T, Task<R>>> cfc && given is Failed<T>    fgc ? fgc.Value.Failed<R>()
               : @this is Failed<Func<T, Task<R>>>    ffd && given is Failed<T>    fgd ? (new AggregateException(ffd.Value.Flat().Concat(fgd.Value.Flat()))).Failed<R>()
               :                                                                   throw new ArgumentException(ApplyAsyncUsageErrorString);

        public static Suspect<Func<T2, TR>> Apply<T1, T2, TR>(this Suspect<Func<T1, T2, TR>> @this, Suspect<T1> given)
            => Apply(@this.Map(Functional.Curry), given);

        public static R Match<T, R>(this Suspect<T> @this, Func<T, R> whenConfirmed, R whenFailed)
            =>   (@this is Confirmed<T> confirmed) ? whenConfirmed(confirmed.Value)
               : (@this is Failed<T> failed)       ? whenFailed
               :                                     throw new Exception(MatchUsageErrorString);

        public static R Match<T, R>(this Suspect<T> @this, Func<T, R> whenConfirmed, Func<R> whenFailed)
            =>   (@this is Confirmed<T> confirmed) ? whenConfirmed(confirmed.Value)
               : (@this is Failed<T> failed)       ? whenFailed()
               :                                     throw new Exception(MatchUsageErrorString);

        public static R Match<T, R>(this Suspect<T> @this, Func<T, R> whenConfirmed, Func<Exception, R> whenFailed)
            =>   (@this is Confirmed<T> confirmed) ? whenConfirmed(confirmed.Value)
               : (@this is Failed<T> failed)       ? whenFailed(failed.Value)
               :                                     throw new Exception(MatchUsageErrorString);

        public static async Task<R> MatchAsync<T, R>(this Suspect<T> @this, Func<T, Task<R>> whenConfirmedAsync, Func<Exception, R> whenFailed)
            =>   (@this is Confirmed<T> confirmed) ? await whenConfirmedAsync(confirmed.Value)
               : (@this is Failed<T> failed)       ? whenFailed(failed.Value)
               :                                     throw new Exception(MatchAsyncUsageErrorString);

        public static Unit Affect<T>(this Suspect<T> @this, Action<T> whenConfirmed, Action<Exception> whenFailed)
        {
            if (@this is Confirmed<T> confirmed)
                whenConfirmed(confirmed.Value);
            else if (@this is Failed<T> failed)
                whenFailed(failed.Value);
            else
                throw new Exception(AffectUsageErrorString);

            return Unit.Value;
        }

        public static async Task<Unit> AffectAsync<T>(this Suspect<T> @this, Func<T, Task<Unit>> whenConfirmedAsync, Func<Exception, Task<Unit>> whenFailedAsync)
        {
            if (@this is Confirmed<T> confirmed)
                return await whenConfirmedAsync(confirmed.Value);
            else if (@this is Failed<T> failed)
                return await whenFailedAsync(failed.Value);
            else
                throw new Exception(AffectAsyncUsageErrorString);
        }

    }

}
