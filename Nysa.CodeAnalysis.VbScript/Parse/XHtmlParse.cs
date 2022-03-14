using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    public class XHtmlParse : Parse
    {
        public HtmlContent                                      Content     { get; private set; }
        public XDocument                                        Document    { get; private set; }
        public IReadOnlyList<(String Source, XElement Element)> VbsIncludes { get; private set; }
        public IReadOnlyList<XHtmlVbScriptParse>                VbsParses   { get; private set; }

        public XHtmlParse(HtmlContent content, XDocument document, IEnumerable<(String Source, XElement)> vbsIncludes, IEnumerable<XHtmlVbScriptParse> vbsParses)
        {
            this.Content        = content;
            this.Document       = document;
            this.VbsIncludes    = vbsIncludes.ToArray();
            this.VbsParses      = vbsParses.ToArray();
        }
    }

}
