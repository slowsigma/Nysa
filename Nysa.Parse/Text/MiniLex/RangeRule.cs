using System;
using System.Collections.Generic;
using System.Text;

namespace Lexingtyn.Definition
{

    // TODO: Was this just a trial balloon? Do we really want this or just the TakeRule functionality that's just like this.
    //       The only reason to keep a RangeRule is if it exist for things higher up the hierarchy than TakeRules (i.e,. literals)
    // TODO: It would be best if the DSL did not allow a RangeRule on top of another RangeRule (which would make no sense).

    public class RangeRule
    {
        private static Int32 ZeroOrMore(Int32 value) => value < 0 ? 0 : value;
        private static Int32 OneOrMore(Int32 value ) => value < 1 ? 1 : value;
        private static (Int32 Minimum, Int32 Maximum) ValidRange(Int32 validMin, Int32 max)
        {
            var validMax = validMin == 0
                           ? max < 1        ? 1        : max    // for a zero minimum, maximum is one or more
                           : max < validMin ? validMin : max;   // for non-zero minimum,  maximum is minimum or mores

            return (validMin, validMax);
        }

        public Int32 Minimum { get; private set; }
        public Int32 Maximum { get; private set; }

        internal RangeRule((Int32 min, Int32 max) range)
        {
            this.Minimum = range.min;
            this.Maximum = range.max;
        }

        public RangeRule Exactly(Int32 count)
            => new RangeRule(ValidRange(OneOrMore(count), count));
        public RangeRule AtLeast(Int32 count)
            => new RangeRule(ValidRange(OneOrMore(count), Int32.MaxValue));
        public RangeRule AtMost(Int32 count)
            => new RangeRule(ValidRange(OneOrMore(count), count));
        public RangeRule From(Int32 minimum, Int32 maximum)
            => new RangeRule(ValidRange(ZeroOrMore(minimum), maximum));

    }

}
