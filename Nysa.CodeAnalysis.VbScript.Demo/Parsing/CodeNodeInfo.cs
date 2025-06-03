using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public record CodeNodeInfo(
        String Title,
        CodeNode? Node,
        IReadOnlyList<CodeNodeInfo> Members,
        Option<Int32> Position,
        Option<Int32> Length);

}