using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript
{

    public class HtmlXmlVbScriptParse : VbScriptParse
    {
        public XElement     Element     { get; private set; }
        public XAttribute?  Attribute   { get; private set; }
        public HtmlXmlVbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot, XElement element, XAttribute? attribute = null)
            : base(content, syntaxRoot)
        {
            this.Element   = element;
            this.Attribute = attribute;
        }
    }

}
