using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public sealed class Try<T>
    {
        public static explicit operator Try<T>(Func<T> function) => new Try<T>(function);

        internal Func<T> Function { get; }
        private Try(Func<T> function) { this.Function = function; }
        public Suspect<T> Eval()
        {
            try { return this.Function().Confirmed(); }
            catch (Exception except) { return except.Failed<T>(); }
        }
        public override String ToString() => $"(Try<{typeof(T).Name}>)";
    }

}
