using System;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    // Note that a Transform function may return nothing (i.e., an empty list).
    public delegate TransformItem[] Transform(TransformContext context, TransformItem[] members);

}