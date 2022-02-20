using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Logics
{

    public sealed class Option
    {
        public static readonly Option None = new Option();

        private Option() { }
    }

}
