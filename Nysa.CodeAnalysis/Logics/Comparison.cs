using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Logics
{

    public static class Comparisons
    {

        public static Boolean SafeEquals<T>(this T first, T second) where T : class, IEquatable<T>
            => Object.ReferenceEquals(first, second) || (!Object.ReferenceEquals(first, null) && first.Equals(second));

    }

}
