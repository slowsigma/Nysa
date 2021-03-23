using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Logics
{

    public sealed class TryAsync<T>
    {
        public static explicit operator TryAsync<T>(Func<Task<T>> function) => new TryAsync<T>(function);

        internal Func<Task<T>> Function { get; }
        private TryAsync(Func<Task<T>> function) { this.Function = function; }
        public async Task<Suspect<T>> EvalAsync()
        {
            try { return (await this.Function()).Confirmed(); }
            catch (Exception except) { return except.Failed<T>(); }
        }
        public override String ToString() => $"(TryAsync<{typeof(T).Name}>)";
    }

}
