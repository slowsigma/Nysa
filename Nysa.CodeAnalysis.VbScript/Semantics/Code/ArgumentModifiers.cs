using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * define("<ArgModifierOpt>", removeEmpty).Is("ByVal")
     *                                        .Or("ByRef")
     *                                        .OrOptional();
     */

    public enum ArgumentModifiers : Int32
    {
        ByVal,
        ByRef
    }

}
