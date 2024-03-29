using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    public record HtmlParse : Parse
    {
        public HtmlContent                          Content     { get; private set; }
        public HtmlDocument                         Document    { get; private set; }
        public IReadOnlyList<HtmlIncludeItem>       VbsIncludes { get; private set; }
        public IReadOnlyList<HtmlVbScriptParse>     VbsParses   { get; private set; }

        public HtmlParse(HtmlContent content, HtmlDocument document, IEnumerable<HtmlIncludeItem> vbsIncludes, IEnumerable<HtmlVbScriptParse> vbsParses)
        {
            this.Content        = content;
            this.Document       = document;
            this.VbsIncludes    = vbsIncludes.ToArray();
            this.VbsParses      = vbsParses.ToArray();
        }
    }

}
