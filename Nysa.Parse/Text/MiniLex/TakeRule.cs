using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing.Mini
{

    public sealed class TakeRule : Rule
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
        
        // instance members
        public CasedString Value { get; private set; }
        public Int32 Minimum { get; private set; }
        public Int32 Maximum { get; private set; }

        internal TakeRule(CasedString value, (Int32 min, Int32 max) range)
        {
            this.Value   = value;
            this.Minimum = range.min;
            this.Maximum = range.max;
        }

        public Rule Exactly(Int32 count)
            => new TakeRule(this.Value, ValidRange(OneOrMore(count), count));
        public Rule AtLeast(Int32 count)
            => new TakeRule(this.Value, ValidRange(OneOrMore(count), Int32.MaxValue));
        public Rule AtMost(Int32 count)
            => new TakeRule(this.Value, ValidRange(OneOrMore(count), count));
        public Rule From(Int32 minimum, Int32 maximum)
            => new TakeRule(this.Value, ValidRange(ZeroOrMore(minimum), maximum));
    }

}
