using System;
using System.Threading.Tasks;

namespace Nysa.Logics
{

    public static class Functional
    {
        public static R Make<T, R>(this T @this, Func<T, R> transform)
            => transform(@this);

        public static Func<R> Make<T, R>(this Func<T> @this, Func<T, R> transform)
            => () => transform(@this());

        public static Func<R> Apply<T, R>(this Func<T, R> @this, T given)
            => () => @this(given);

        public static Func<T2, TR> Apply<T1, T2, TR>(this Func<T1, T2, TR> @this, T1 given)
            => t2 => @this(given, t2);

        public static Func<T2, T3, TR> Apply<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> @this, T1 given)
            => (t2, t3) => @this(given, t2, t3);

        public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(this Func<T1, T2, TR> @this)
            => t1 => t2 => @this(t1, t2);

        public static Func<T1, Func<T2, Task<TR>>> Curry<T1, T2, TR>(this Func<T1, T2, Task<TR>> @thisAsync)
            => t1 => async t2 => await @thisAsync(t1, t2);

        public static Func<T1, Func<T2, Func<T3, TR>>> Curry<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> @this)
            => t1 => t2 => t3 => @this(t1, t2, t3);

        public static Func<T1, Func<T2, Func<T3, Task<TR>>>> Curry<T1, T2, T3, TR>(this Func<T1, T2, T3, Task<TR>> @thisAsync)
            => t1 => t2 => async t3 => await @thisAsync(t1, t2, t3);

        public static Func<R> Bind<T, R>(this Func<T> @this, Func<T, Func<R>> transform)
            => () => transform(@this())();

    }

}
