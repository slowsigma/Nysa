using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * define("<LoopType>").Is("While")
     *                     .Or("Until");
     */

    public enum LoopTypes : Int32
    {
        While,
        Until
    }

}
