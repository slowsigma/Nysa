using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public sealed class Some<T> : Option<T>
    {
        public static explicit operator Some<T>(T value) => new Some<T>(value);
        public T Value { get; private set; }
        public Some(T value) { this.Value = value; }
        public override String ToString() => this.Value.ToString();
    }

}
