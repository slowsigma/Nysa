using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public abstract class Option<T>
        where T : notnull
    {
        public static readonly Option<T> None = new None<T>();
        public static implicit operator Option<T>(Option option) => Option<T>.None;

        [Obsolete("Misleading name, use Affect instead.")]
        public Unit Send(Action<T> action)
        {
            if (this is Some<T> some)
                action(some.Value);

            return Unit.Value;
        }

        /// <summary>
        /// Takes an Option of some type and calls an update
        /// action if the Option is Some of that type.
        /// </summary>
        public Unit Affect(Action<T> update)
        {
            if (this is Some<T> some)
                update(some.Value);

            return Unit.Value;
        }
    }

    public class Option
    {
        public static readonly Option None = new Option();
        private Option() { }
    }

}
