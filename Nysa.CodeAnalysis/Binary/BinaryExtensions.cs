using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Binary
{

    public static class BinaryExtensions
    {

        public static Int32 ToBitFlag(this Byte bitIndex) { return (Int32)Math.Pow(2, bitIndex % 32); }
        public static Int32 ToBitFlag(this Int32 bitIndex) { return (Int32)Math.Pow(2, bitIndex % 32); }

        public static Int32 ToInt32(this IEnumerable<Boolean> bits)
        {
            // each true/false value is represented in the result by a bit
            return bits.Where((v, i) => i < 31)
                       .Select((v, i) => v ? ((Byte)i).ToBitFlag() : 0)
                       .Aggregate(0, (a, p) => a | p);
        }

        public static Int32 ToInt32(this Boolean[] bits)
        {
            // each true/false value is represented in the result by a bit
            return bits.Where((v, i) => i < 31)
                       .Select((v, i) => v ? ((Byte)i).ToBitFlag() : 0)
                       .Aggregate(0, (a, p) => a | p);
        }

        public static IEnumerable<Boolean> ToBits(this Int32 flags)
        {
            for (UInt32 i = 1; i < 2147483648; i *= 2)
                yield return (flags & i) != 0;
        }

    }

}
