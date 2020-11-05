using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Text.Parsing.Definition
{

    public class Sequence : Pattern
    {
        public String           Value    { get; private set; }
        public StringComparer   Comparer { get; private set; }

        public Sequence(String value, StringComparer comparer = null)
        {
            this.Value    = value;
            this.Comparer = comparer ?? StringComparer.Ordinal;
        }
    }

}
