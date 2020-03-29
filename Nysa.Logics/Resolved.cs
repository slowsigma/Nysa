using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public sealed class Resolved<T> : Try<T>
    {
        public static implicit operator Resolved<T>(Suspect<T> value) => new Resolved<T>(value);

        public Suspect<T> Value { get; }
        private Resolved(Suspect<T> value) { this.Value = value; }
        public override String ToString() => this.Value.ToString();
    }

}
