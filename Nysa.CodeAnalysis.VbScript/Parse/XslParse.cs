using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nysa.CodeAnalysis.VbScript
{
    
    public class XslParse : Parse
    {
        public XslContent                       Content     { get; private set; }
        public XDocument                        Document    { get; private set; }
        public IReadOnlyList<XslVbScriptParse>  VbParses    { get; private set; }

        public XslParse(XslContent content, XDocument document, IEnumerable<XslVbScriptParse> vbParses)
        {
            this.Content    = content;
            this.Document   = document;
            this.VbParses   = vbParses.ToArray();
        }
    }

}
