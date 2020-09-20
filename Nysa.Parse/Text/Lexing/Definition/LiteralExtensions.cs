using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Text.Lexing.Definition
{

    public static class LiteralExtensions
    {
        public static CasedString Insensitive(this String value) => new CasedString(value, true);

        /// <summary>
        /// The purpose of SetConcat is to make a larger set of characters from two smaller sets.
        /// The resulting CasedString is used only in an EitherRule as the set of characters the
        /// rule accepts or the set of characters the rule does not accept.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        internal static CasedString SetConcat(this CasedString @this, CasedString other)
            => new CasedString(String.Concat(@this.IsInsensitive ? @this.Value.Expand() : @this.Value,
                                             other.IsInsensitive ? other.Value.Expand() : other.Value),
                               @this.IsInsensitive && other.IsInsensitive);

        /// <summary>
        /// Returns a string containing all the characters provided in the value parameter as
        /// well as any upper case or lower case alternatives to those found in the value
        /// parameter.  For example, if the value parameter equals "abCD%?", then the result of
        /// Expand is "abCD%?ABcd". The order of the characters returned is not guaranteed and
        /// should not matter.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String Expand(this String value)
        {
            var build = new StringBuilder();
            build.Append(value);

            foreach (var item in value)
            {
                if (Char.IsLower(item))
                    build.Append(Char.ToUpper(item));
                else if (Char.IsUpper(item))
                    build.Append(Char.ToLower(item));
            }

            return build.ToString();
        }
    }

}
