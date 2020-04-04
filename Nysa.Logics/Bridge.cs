using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public static class Bridge
    {
        public static Unit Send<T>(this T @this, Action<T> action)
        {
            action(@this);
            return Unit.Value;
        }

        public static Unit Send<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
                action(item);

            return Unit.Value;
        }
    }

}
