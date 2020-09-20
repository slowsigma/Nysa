using System;
using System.Collections.Generic;

namespace Nysa.Text.Lexing.Mini
{

    /// <summary>
    /// OrRule contains two member rules for patterns of text
    /// that may be found at a given point in a search. In
    /// action, the function representing the OrRule would
    /// take the current search state, try each member rule,
    /// and return whatever match was longest amongst the
    /// alternatives.
    /// </summary>
    public sealed class OrRule : Rule
    {
        public Rule First  { get; private set; }
        public Rule Second { get; private set; }

        internal OrRule(Rule first, Rule second)
        {
            this.First  = first;
            this.Second = second;
        }
    }

}
