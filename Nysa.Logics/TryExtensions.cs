using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public static class TryExtensions
    {
        private static readonly String UsageErrorTemplate = "{0} does not accept types other than Pending<T> and Resolved<T>.";
        private static readonly String MapUsageErrorString = String.Format(UsageErrorTemplate, nameof(Map));
        private static readonly String BindUsageErrorString = String.Format(UsageErrorTemplate, nameof(Bind));
        private static readonly String ApplyUsageErrorString = String.Format(UsageErrorTemplate, nameof(Apply));
        private static readonly String MatchUsageErrorString = String.Format(UsageErrorTemplate, nameof(Match));

        public static Try<R> Map<T, R>(this Try<T> @this, Func<T, R> transform)
            =>   @this is Pending<T>  pending  ? pending.Run().Match(pc => transform(pc).Confirmed().Resolved(),
                                                                     pe => pe.Failed<R>().Resolved())
               : @this is Resolved<T> resolved ? resolved.Value.Match(rc => transform(rc).Confirmed().Resolved(),
                                                                      re => re.Failed<R>().Resolved())
               :                                 throw new ArgumentException(MapUsageErrorString, nameof(@this));

        public static Try<R> Bind<T, R>(this Try<T> @this, Func<T, Try<R>> transform)
            =>   @this is Pending<T>  pending  ? pending.Run().Match(pc => transform(pc),
                                                                     pe => pe.Failed<R>().Resolved())
               : @this is Resolved<T> resolved ? resolved.Value.Match(rc => transform(rc),
                                                                      re => re.Failed<R>().Resolved())
               :                                 throw new ArgumentException(BindUsageErrorString, nameof(@this));

        public static Try<R> Apply<T, R>(this Try<Func<T, R>> @this, Try<T> given)
            =>   @this is Pending<Func<T, R>>  pfa && given is Pending<T>  pga ? pfa.Run().Apply(pga.Run()).Resolved()
               : @this is Resolved<Func<T, R>> rfb && given is Pending<T>  pgb ? rfb.Value.Apply(pgb.Run()).Resolved()
               : @this is Pending<Func<T, R>>  pfc && given is Resolved<T> rgc ? pfc.Run().Apply(rgc.Value).Resolved()
               : @this is Resolved<Func<T, R>> rfd && given is Resolved<T> rgd ? rfd.Value.Apply(rgd.Value).Resolved()
               :                                                                 throw new ArgumentException(ApplyUsageErrorString);

        public static Try<Func<T2, TR>> Apply<T1, T2, TR>(this Try<Func<T1, T2, TR>> @this, Try<T1> given)
            => Apply(@this.Map(Functional.Curry), given);

        public static R Match<T, R>(this Try<T> @this, Func<T, R> whenConfirmed, R whenFailed)
            =>   @this is Pending<T>  pending  ? pending.Run().Match(pc => whenConfirmed(pc), pe => whenFailed)
               : @this is Resolved<T> resolved ? resolved.Value.Match(rc => whenConfirmed(rc), re => whenFailed)
               :                                 throw new ArgumentException(MatchUsageErrorString, nameof(@this));

        public static R Match<T, R>(this Try<T> @this, Func<T, R> whenConfirmed, Func<R> whenFailed)
            =>   @this is Pending<T>  pending  ? pending.Run().Match(pc => whenConfirmed(pc), pe => whenFailed())
               : @this is Resolved<T> resolved ? resolved.Value.Match(rc => whenConfirmed(rc), re => whenFailed())
               :                                 throw new ArgumentException(MatchUsageErrorString, nameof(@this));
    }

}
