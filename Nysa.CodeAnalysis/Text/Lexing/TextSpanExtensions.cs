using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;

namespace Nysa.Text
{

    /// <summary>
    /// Represents any function that accepts zero, one, or more characters at the trailing edge of an
    /// existing TextSpan. The standard convention is that an Accept normally either returns a null or
    /// it expands the given TextSpan by the amount of characters the function accepts starting at the
    /// end of the given TextSpan.
    /// </summary>
    /// <param name="span"></param>
    /// <returns></returns>
    public delegate TextSpan? Accept(TextSpan? span);

    public static class TextSpanExtensions
    {

        public static TextSpan Start(this String source)
            => ((TextSpan)source).Start;

        public static TextSpan End(this String source)
            => ((TextSpan)source).End;

    }

}
