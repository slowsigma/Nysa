using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public sealed class Pending<T> : Try<T>
    {
        public static explicit operator Pending<T>(Func<T> value) => new Pending<T>(value);

        private Func<T> Value { get; }
        private Pending(Func<T> value) { this.Value = value; }
        public Suspect<T> Run()
        {
            try { return this.Value().Confirmed(); }
            catch (Exception except) { return except.Failed<T>(); }
        }
        public override String ToString() => $"(Pending<{typeof(T).Name}>)";
    }

}
