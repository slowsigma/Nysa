using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text;

public static class TextSpanFunctions
{

    public static TextSpan Appended(this TextSpan @this, Int32 length)
        => new TextSpan(@this.Source, @this.Position, @this.Length + length);

    public static TextSpan StartAfter(this TextSpan @this)
        => new TextSpan(@this.Source, @this.End.Value, 0);

    // public static TextSpan Appended(this TextSpan @this, Func<Char, Boolean> whileTrue)
    // {
    //     var check = @this.Position + @this.Length;

    //     while (check < @this.Source.Length && whileTrue(@this.Source[check]))
    //         check++;

    //     return new TextSpan(@this.Source, @this.Position, check - 1);
    // }

}