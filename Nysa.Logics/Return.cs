using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public static class Return
    {
        public static T After<T>(Action action, Func<T> function)
        {
            action();
            return function();
        }

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

        public static Suspect<T> Confirmed<T>(this T @this)
            => new Confirmed<T>(@this);

        public static Suspect<T> ConfirmedIf<T>(this T @this, Func<T, Boolean> isValid, String errorMessage)
            => isValid(@this) ? @this.Confirmed()
               : (Failed<T>)(new Exception(errorMessage));

        public static Suspect<T> Failed<T>(this Exception @this)
            => new Failed<T>(@this);

        public static Try<T> Pending<T>(this Func<T> @this)
            => (Pending<T>)@this;

        public static Try<T> Resolved<T>(this Suspect<T> @this)
            => (Resolved<T>)@this;

        public static IEnumerable<T> Enumerable<T>(this T @this)
        {
            yield return @this;
        }
    }

}
