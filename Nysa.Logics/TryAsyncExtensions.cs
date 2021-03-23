using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Logics
{

    public static class TryAsyncExtensions
    {
        public static TryAsync<R> Map<T, R>(this TryAsync<T> @this, Func<T, Task<R>> transform)
            => (TryAsync<R>)(async () => await transform(await @this.Function()));

        public static async Task<R> MatchAsync<T, R>(this TryAsync<T> @this, Func<T, R> whenConfirmed, R whenFailed)
            => (await @this.EvalAsync()).Match(whenConfirmed, whenFailed);

        public static async Task<R> MatchAsync<T, R>(this TryAsync<T> @this, Func<T, R> whenConfirmed, Func<R> whenFailed)
            => (await @this.EvalAsync()).Match(whenConfirmed, whenFailed);

        public static async Task<R> MatchAsync<T, R>(this TryAsync<T> @this, Func<T, R> whenConfirmed, Func<Exception, R> whenFailed)
            => (await @this.EvalAsync()).Match(whenConfirmed, whenFailed);
    }

}
