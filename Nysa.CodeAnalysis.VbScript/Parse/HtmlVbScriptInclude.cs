using System;
using System.Collections.Generic;
using System.Text;

namespace CodeAnalysis.VbScript
{

    public class HtmlVbScriptInclude
    {
        public String ElementPath   { get; private set; }
        public String Source        { get; private set; }

        public HtmlVbScriptInclude(String elementPath, String source)
        {
            this.ElementPath    = elementPath;
            this.Source         = source;
        }
    }

}
