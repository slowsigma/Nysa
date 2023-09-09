using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Logics
{

    public static class Args
    {

        public static IEnumerable<Arg> AsArgs(this String[] @this)
        {
            var priorName   = (String?)null;
            var priorUsed   = false;

            for (Int32 i = 0; i < @this.Length; i++)
            {
                var current = @this[i];

                // is current a name?
                if ((current.StartsWith("-") || current.StartsWith("/")) && current.Length > 1 && Char.IsLetter(current[1]))
                {
                    // did we have a name before that was not used?
                    if ((priorName != null) && !priorUsed)
                        yield return new FlagArg(priorName); // yes, return it as a flag

                    // now we have an unused name
                    priorName = current;
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

    }

}