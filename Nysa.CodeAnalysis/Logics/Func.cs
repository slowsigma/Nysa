using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Logics
{

    public static class Func
    {
        public static Func<T> OfT<T>(Func<T> function) => function;
    }

}
