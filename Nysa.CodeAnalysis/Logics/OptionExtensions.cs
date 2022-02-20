using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Logics
{

    public static class OptionExtensions
    {
        public static Option<T> IfTrue<T>(this Boolean test, Func<T> trueValue)
            => test ? trueValue() : Option<T>.None;

        public static Option<T> ToOption<T>(this T some)
            => some;
    }

}
