using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Logics
{

    public static class Enums
    {
        public static IEnumerable<T> Return<T>(this T value) { yield return value; }

        public static IEnumerable<T> None<T>() { yield break; }

    }

}
