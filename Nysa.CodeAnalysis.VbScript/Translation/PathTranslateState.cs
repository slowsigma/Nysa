using System;

namespace Nysa.CodeAnalysis.VbScript;

public enum PathTranslateState
{
    Start,
    Variable,
    Constant,
    Property,
    Function,
    FunctionArguments,
    FinalArguments,
    ArrayArguments,
    ArrayIntializer,
    GetRef,
    ClassName,
    End
}
