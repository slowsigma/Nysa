using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Logics
{

    public static class TryExtensions
    {
        public static Try<R> ThenTry<T, R>(this Try<T> @this, Func<T, R> transform)
            => Return.Try(() => transform(@this.Function()));

        public static Suspect<R> Map<T, R>(this Try<T> @this, Func<T, R> transform)
            => @this.Eval().Map(t => transform(t));

        public static Suspect<R> Bind<T, R>(this Try<T> @this, Func<T, Suspect<R>> transform)
            => @this.Eval().Bind(transform);

        public static R Match<T, R>(this Try<T> @this, Func<T, R> whenConfirmed, R whenFailed)
            => @this.Eval().Match(whenConfirmed, whenFailed);

        public static R Match<T, R>(this Try<T> @this, Func<T, R> whenConfirmed, Func<R> whenFailed)
            => @this.Eval().Match(whenConfirmed, whenFailed);

        public static R Match<T, R>(this Try<T> @this, Func<T, R> whenConfirmed, Func<Exception, R> whenFailed)
            => @this.Eval().Match(whenConfirmed, whenFailed);
    }

}
