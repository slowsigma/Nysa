using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing.Mini
{

    /// <summary>
    /// ThenRule contains two member patterns of text
    /// where the first must be found at some point
    /// in a search then the second must be found
    /// directly after the first is matched. In action,
    /// the function representing the TheRule will
    /// return a single match that spans both the
    /// first and second members when they're matched
    /// correctly.
    /// </summary>
    public sealed class ThenRule : Rule
    {
        public Rule First  { get; private set; }
        public Rule Second { get; private set; }

        internal ThenRule(Rule first, Rule second)
        {
            this.First  = first;
            this.Second = second;
        }
    }

}
