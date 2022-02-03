using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * define("<PropertyAccessType>").Is("Get")
     *                               .Or("Let")
     *                               .Or("Set");
     */

    public enum PropertyAccessTypes : Int32
    {
        Get,
        Let,
        Set
    }

}
