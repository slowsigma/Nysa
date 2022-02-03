using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public enum ConstantOperationTypes : Int32
    {
        precedence, // sub-operation indicated with paranthesis
        add,
        subtract
    }

}
