using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <AccessModifierOpt> ::=   "Public"
     *                         | "Private"
     *                         | 
     */

    public enum VisibilityTypes : Int32
    {
        Public,
        Private
    }

}
