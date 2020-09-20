using System;

namespace Nysa.Text.Lexing.Mini
{

    /// <summary>
    /// UntilRule contains lexical patterns of text
    /// that signifies when to stop matching text. In
    /// action, the function representing UntilRule
    /// will try to match the What property and if it
    /// fails the function will expand the current
    /// state by one. If the function runs out of
    /// input or finds a match on the What property,
    /// it will then return the working state up to,
    /// but not including that position.  That is,
    /// the position and length of the returned state
    /// will never exceed the bounds of the input
    /// and will not include any of the matched What
    /// state.
    /// </summary>
    public sealed class UntilRule : Rule
    {
        public Rule What { get; private set; }

        internal UntilRule(Rule what) { this.What = what; }
    }

}
