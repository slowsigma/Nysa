using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public abstract class Option<T>
    {
        public static readonly Option<T> None = new None<T>();
        public static implicit operator Option<T>(Option option) => Option<T>.None;

        public Unit Send(Action<T> action)
        {
            if (this is Some<T> some)
                action(some.Value);

            return Unit.Value;
        }
    }

    public class Option
    {
        public static readonly Option None = new Option();
        private Option() { }
    }

}
