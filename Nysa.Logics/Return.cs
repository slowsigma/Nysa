using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public static class Return
    {
        public static Lazy<T> Lazy<T>(this Func<T> @this, Boolean isThreadSafe = false)
            => new Lazy<T>(@this, isThreadSafe);

        public static Try<T> Try<T>(this Func<T> @this)
            => (Try<T>)@this;

        public static Option<T> Some<T>(this T value)
            => new Some<T>(value);

        public static Option<T> NoneIf<T>(this T value, Func<T, Boolean> isNone)
            => isNone(value) ? Option<T>.None : value.Some();

        public static Option<T> AsOption<T>(this T value, Boolean noneIfNull = true)
            where T : class
            => noneIfNull && value == null ? Option<T>.None
               : value.Some();

        public static Option<T> AsOption<T>(this Nullable<T> value, Boolean noneIfNull = true)
            where T : struct
            => noneIfNull && value == null ? Option<T>.None
               : value.Value.Some();
    }

}
