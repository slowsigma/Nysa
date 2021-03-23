using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Logics
{

    public static class TryExtensions
    {
        public static Try<R> Map<T, R>(this Try<T> @this, Func<T, R> transform)
            => (Try<R>)(() => transform(@this.Function()));

        public static R Match<T, R>(this Try<T> @this, Func<T, R> whenConfirmed, R whenFailed)
            => @this.Eval().Match(whenConfirmed, whenFailed);

        public static R Match<T, R>(this Try<T> @this, Func<T, R> whenConfirmed, Func<R> whenFailed)
            => @this.Eval().Match(whenConfirmed, whenFailed);

        public static R Match<T, R>(this Try<T> @this, Func<T, R> whenConfirmed, Func<Exception, R> whenFailed)
            => @this.Eval().Match(whenConfirmed, whenFailed);
    }

}
