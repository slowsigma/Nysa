using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public record ViewInfo(String Title, CodeNode Node, Func<IEnumerable<ViewInfo>> Children);

}