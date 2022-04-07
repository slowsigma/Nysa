using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nysa.CodeAnalysis.VbScript
{
    
    public class XslParse : Parse
    {
        public XslContent                       Content     { get; private set; }
        public XmlDocument                      Document    { get; private set; }
        public IReadOnlyList<XslVbScriptParse>  VbParses    { get; private set; }

        public XslParse(XslContent content, XmlDocument document, IEnumerable<XslVbScriptParse> vbParses)
        {
            this.Content    = content;
            this.Document   = document;
            this.VbParses   = vbParses.ToArray();
        }
    }

}
