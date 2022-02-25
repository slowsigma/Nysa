using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Text;

namespace Nysa.CodeAnalysis.Visualizer
{

    public static class SymbolConstants
    {

        public static readonly HashSet<Identifier> ReservedWords = new HashSet<Identifier>(
            (new Int32[] { 112,113,114,115,116,117,118,119,120,126,
                          127,128,129,130,133,134,136,137,138,139,
                          141,142,143,144,145,146,147,148,149,150,
                          151,152,153,154,155,156,157,158,159,160,
                          164,165,166,167,168,169,170,171,180,189,
                          190,193,194,195,196,197,198,199          }).Select(n => Identifier.FromInteger(n)));

    }

}
