using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    public class HtmlParse : Parse
    {
        public HtmlContent                                      Content     { get; private set; }
        public HtmlDocument                                     Document    { get; private set; }
        public IReadOnlyList<(String Source, HtmlNode Node)>    VbsIncludes { get; private set; }
        public IReadOnlyList<HtmlVbScriptParse>                 VbsParses   { get; private set; }

        public HtmlParse(HtmlContent content, HtmlDocument document, IEnumerable<(String Source, HtmlNode Node)> vbsIncludes, IEnumerable<HtmlVbScriptParse> vbsParses)
        {
            this.Content        = content;
            this.Document       = document;
            this.VbsIncludes    = vbsIncludes.ToArray();
            this.VbsParses      = vbsParses.ToArray();
        }
    }

}
