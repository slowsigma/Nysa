﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public static Option<T> Some<T>(this T value)
            => new Some<T>(value);

#if NET
#nullable enable

        /// <summary>
        /// Given a possible null value of some type, this function returns
        /// the None subtype of Option if the value is null or if the
        /// supplied isNone function returns true for the value. Otherwise,
        /// this function returns the Some subtype of Option with the
        /// supplied value.
        /// </summary>
        public static Option<T> NoneIf<T>(this T? value, Func<T, Boolean> isNone)
            =>   value == null ? Option<T>.None
               : isNone(value) ? Option<T>.None
               :                 value.Some();

#else

        /// <summary>
        /// Given a value of some type, this function returns the
        /// None subtype of Option if the supplied isNone function
        /// returns true for the value. Otherwise, this function
        /// returns the Some subtype of Option with the supplied
        /// value.
        /// </summary>
        public static Option<T> NoneIf<T>(this T value, Func<T, Boolean> isNone)
            => isNone(value) ? Option<T>.None : value.Some();
#endif

        public static Option<T> AsOption<T>(this T value, Boolean noneIfNull = true)
            where T : class
            => noneIfNull && value == null ? Option<T>.None
               : value.Some();

        public static Option<T> AsOption<T>(this Nullable<T> value)
            where T : struct
            => value == null ? Option<T>.None : value.Value.Some();

        public static Suspect<T> Confirmed<T>(this T @this)
            => new Confirmed<T>(@this);

        public static Suspect<T> ConfirmedIf<T>(this T @this, Func<T, Boolean> isValid, String errorMessage)
            => isValid(@this) ? @this.Confirmed()
               : (Failed<T>)(new Exception(errorMessage));

        public static Suspect<T> Failed<T>(this Exception @this)
            => new Failed<T>(@this);

        public static Suspect<T> Failed<T>(String errorMessage)
            => (new Exception(errorMessage)).Failed<T>();

        public static Suspect<T> Try<T>(this Func<T> @this)
        {
            try { return @this().Confirmed(); }
            catch (Exception except) { return except.Failed<T>(); }
        }

        public static Suspect<Unit> Try(this Action @this)
            => (new Func<Unit>(() => { @this(); return Unit.Value; })).Try();

        public static async Task<Suspect<T>> TryAsync<T>(this Func<Task<T>> @this)
        {
            try { return (await @this()).Confirmed(); }
            catch (Exception except) { return except.Failed<T>(); }
        }

        public static IEnumerable<T> Enumerable<T>(this T @this, params T[] more)
        {
            yield return @this;

            foreach (var item in more)
                yield return item;
        }

        public static IEnumerable<T> Some<T>(params Option<T>[] options)
        {
            foreach (var option in options)
                if (option is Some<T> some)
                    yield return some.Value;
        }

    }

}
