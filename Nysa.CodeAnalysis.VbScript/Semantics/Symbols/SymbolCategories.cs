using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class SymbolCategories
    {
        public static readonly String vb        = "vb";         // only used for the err type
        public static readonly String com       = "com";
        public static readonly String page      = "page";
        public static readonly String parent    = "parent";
        public static readonly String style     = "style";      // a sub-category of page
    }

}