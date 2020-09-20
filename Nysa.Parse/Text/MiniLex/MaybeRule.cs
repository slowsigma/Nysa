using System;

namespace Nysa.Text.Lexing.Mini
{

    /// <summary>
    /// MaybeRule contains a lexical rule for a pattern of
    /// text that may or may not be found at a given point
    /// in a search. In action, the function representing
    /// the MaybeRule would take the current search state
    /// attempt to match based on the What property and if
    /// it matches, the function expands the state to
    /// cover the match or it returns the state with no
    /// change. Regardless, the function never returns
    /// failure.
    /// </summary>
    public sealed class MaybeRule : Rule
    {
        public Rule What { get; private set; }

        internal MaybeRule(Rule what) { this.What = what; }
    }

}
