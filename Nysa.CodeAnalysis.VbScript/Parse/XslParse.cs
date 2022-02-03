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
        public XElement                         Root        { get; private set; }
        public IReadOnlyList<XslVbScriptParse>  VbParses    { get; private set; }

        public XslParse(XslContent content, XElement root, IEnumerable<XslVbScriptParse> vbParses)
        {
            this.Content    = content;
            this.Root       = root;
            this.VbParses   = vbParses.ToArray();
        }
    }

}
