using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    public record XHtmlParse : Parse
    {
        public HtmlContent                                          Content     { get; private set; }
        public XmlDocument                                          Document    { get; private set; }
        public IReadOnlyList<(String Source, XmlElement Element)>   VbsIncludes { get; private set; }
        public IReadOnlyList<XHtmlVbScriptParse>                    VbsParses   { get; private set; }

        public XHtmlParse(HtmlContent content, XmlDocument document, IEnumerable<(String Source, XmlElement)> vbsIncludes, IEnumerable<XHtmlVbScriptParse> vbsParses)
        {
            this.Content        = content;
            this.Document       = document;
            this.VbsIncludes    = vbsIncludes.ToArray();
            this.VbsParses      = vbsParses.ToArray();
        }
    }

}
