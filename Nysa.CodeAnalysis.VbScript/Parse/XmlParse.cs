using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nysa.CodeAnalysis.VbScript
{
    
    public record XmlParse : Parse
    {
        public XmlContent                       Content     { get; private set; }
        public XmlDocument                      Document    { get; private set; }
        public IReadOnlyList<XmlVbScriptParse>  VbParses    { get; private set; }

        public XmlParse(XmlContent content, XmlDocument document, IEnumerable<XmlVbScriptParse> vbParses)
        {
            this.Content    = content;
            this.Document   = document;
            this.VbParses   = vbParses.ToArray();
        }
    }

}
