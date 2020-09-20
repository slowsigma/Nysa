using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Text
{

    public static class TextExtensions
    {
        public static Boolean DataEquals(this String value, String other)
            => value == null ? (other == null) : value.Equals(other, StringComparison.OrdinalIgnoreCase);
        public static Boolean DataStartsWith(this String value, String starting)
            => value == null ? false : value.StartsWith(starting, StringComparison.OrdinalIgnoreCase);
        public static Boolean DataEndsWith(this String value, String ending)
            => value == null ? false : value.EndsWith(ending, StringComparison.OrdinalIgnoreCase);
        public static Int32 DataIndexOf(this String value, String find, Int32 startIndex = 0)
            => value == null ? -1 : value.IndexOf(find, startIndex, StringComparison.OrdinalIgnoreCase);
    }

}
