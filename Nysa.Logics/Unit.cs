using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{

    public sealed class Unit
    {
        public static readonly Unit Value = new Unit();

        private Unit() { }
    }

}
