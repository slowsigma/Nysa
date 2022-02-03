using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace CodeAnalysis.VbScript
{

    public class HtmlParse : Parse
    {
        public HtmlContent                          Content         { get; private set; }
        public HtmlDocument                         HtmlDocument    { get; private set; }
        public IReadOnlyList<HtmlVbScriptInclude>   VbIncludes      { get; private set; }
        public IReadOnlyList<HtmlVbScriptParse>     VbParses        { get; private set; }

        public HtmlParse(HtmlContent content, HtmlDocument htmlDocument, IEnumerable<HtmlVbScriptInclude> vbIncludes, IEnumerable<HtmlVbScriptParse> vbScriptParses)
        {
            this.Content        = content;
            this.HtmlDocument   = htmlDocument;
            this.VbIncludes     = vbIncludes.ToArray();
            this.VbParses       = vbScriptParses.ToArray();
        }
    }

}
