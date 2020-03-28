using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{
    
    public sealed class Confirmed<T> : Suspect<T>
    {
        public static implicit operator T(Confirmed<T> confirmed) => confirmed.Value;

        public T Value { get; }
        public Confirmed(T value) { this.Value = value; }
        public override String ToString() => this.Value.ToString();
    }

}
