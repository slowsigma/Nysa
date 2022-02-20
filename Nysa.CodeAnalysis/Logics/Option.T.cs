using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Logics
{

    [DebuggerDisplay("{ToString()}")]
    public struct Option<T>
    {
        public static readonly Option<T> None = new Option<T>(false, default(T));

        public static implicit operator Option<T>(T some) { return new Option<T>(true, some); }

        public static implicit operator Option<T>(Option option) { return None; }

        public T        Value   { get; private set; }
        public Boolean  IsSome  { get; private set; }
        public Boolean  IsNone  { get { return !this.IsSome; } }

        private Option(Boolean isSome, T value)
        {
            this.IsSome = isSome;
            this.Value  = value;
        }

        public Option<U> Select<U>(Func<T, U> selector) => this.IsNone ? Option.None : (Option<U>)selector(this.Value);

        public Option<U> Select<U>(Func<T, Option<U>> selector) => this.IsNone ? Option.None : selector(this.Value);

        public U Select<U>(Func<T, U> selector, U @default) => this.IsNone ? @default : selector(this.Value);

        public U Select<U>(Func<T, U> selector, Func<U> @default) => this.IsNone ? @default() : selector(this.Value);

        public Option<T> OrOther(Option<T> @default) => this.IsNone ? @default : this;
        public T OrValue(T @default) => this.IsNone ? @default : this.Value;

        public override string? ToString() => this.IsNone ? "{Option.None}" : this.Value?.ToString();

    }

}
