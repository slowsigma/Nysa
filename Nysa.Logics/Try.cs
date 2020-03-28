using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public abstract class Try<T>
    {
        public static explicit operator Try<T>(Func<T> value) => (Pending<T>)value;
        public static implicit operator Try<T>(Suspect<T> value) => (Resolved<T>)value;

        public abstract Suspect<T> Run();
    }

}
