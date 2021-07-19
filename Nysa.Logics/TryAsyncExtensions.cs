using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Logics
{

    public static class TryAsyncExtensions
    {
        public static TryAsync<R> ThenTryAsync<T, R>(this TryAsync<T> @this, Func<T, Task<R>> transform)
            => Return.TryAsync(async () => await transform(await @this.Function()));

        public static async Task<Suspect<R>> MapAsync<T, R>(this TryAsync<T> @this, Func<T, R> transform)
            => (await @this.EvalAsync()).Map(t => transform(t));

        public static async Task<Suspect<R>> BindAsync<T, R>(this TryAsync<T> @this, Func<T, Suspect<R>> transform)
            => (await @this.EvalAsync()).Bind(transform);

        public static async Task<R> MatchAsync<T, R>(this TryAsync<T> @this, Func<T, R> whenConfirmed, R whenFailed)
            => (await @this.EvalAsync()).Match(whenConfirmed, whenFailed);

        public static async Task<R> MatchAsync<T, R>(this TryAsync<T> @this, Func<T, R> whenConfirmed, Func<R> whenFailed)
            => (await @this.EvalAsync()).Match(whenConfirmed, whenFailed);

        public static async Task<R> MatchAsync<T, R>(this TryAsync<T> @this, Func<T, R> whenConfirmed, Func<Exception, R> whenFailed)
            => (await @this.EvalAsync()).Match(whenConfirmed, whenFailed);
    }

}
