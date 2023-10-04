using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Logics
{

    public static class Args
    {
        public static IReadOnlyList<String> StandardNamePrefixes = new String[] { "-", "/", "--" };

        public static IEnumerable<Arg> Parse(IReadOnlyList<String> namePrefixes, Boolean stripNamePrefix, String[] @this)
        {
            if (namePrefixes.Any(p => p.Any(c => Char.IsLetter(c))))
                throw new ArgumentException("Letters (e.g., 'a'..'z') are not allowed as name prefix characters.", nameof(namePrefixes));

            var priorName   = (String?)null;
            var priorUsed   = false;

            for (Int32 i = 0; i < @this.Length; i++)
            {
                var current = @this[i];
                var prefix  = namePrefixes.FirstOrNone(p => current.StartsWith(p));

                // is current a name?
                if (prefix is Some<String> somePrefix && Char.IsLetter(current[somePrefix.Value.Length]))
                {
                    // did we have a name before that was not used?
                    if ((priorName != null) && !priorUsed)
                        yield return new FlagArg(priorName); // yes, return it as a flag

                    // now we have an unused name
                    priorName = stripNamePrefix
                                ? current.Substring(somePrefix.Value.Length)
                                : current;
                    priorUsed = false;
                }
                else // current is not an argument name
                {
                    yield return priorName == null
                                 ? new LooseArg(current)                // no name?   loose argument value
                                 : new ValueArg(priorName, current);    // otherwise, named value argument

                    priorUsed = priorName != null;
                }
            }

            // do we have an unused name to yield?
            if ((priorName != null) && !priorUsed)
                yield return new FlagArg(priorName);
        }

        public static IEnumerable<Arg> AsArgs(this String[] @this) => Parse(StandardNamePrefixes, true, @this);

    }

}