using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text;

public static class TextPositionFunctions
{

    public static Option<Char> Char(this TextPosition @this)
        => !@this.IsSourceEnd() ? @this.Source[@this.Value].Some() : Option<Char>.None;

}