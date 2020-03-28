using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{
    public sealed class Failed<T> : Suspect<T>
    {
        public static explicit operator Failed<T>(Exception value) => new Failed<T>(value);
        public static implicit operator Exception(Failed<T> failed) => failed.Value;

        public Exception Value { get; }
        public Failed(Exception value) { this.Value = value; }
        public override String ToString() => this.Value.Message;
    }
}
