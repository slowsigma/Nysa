using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public static class TryExtensions
    {
        public static Try<R> Bind<T, R>(this Try<T> @this, Func<T, Try<R>> transform)
            => @this.Run()
                    .Match(c => transform(c),
                           f => (Try<R>)(f.Failed<R>()));

    }

}
