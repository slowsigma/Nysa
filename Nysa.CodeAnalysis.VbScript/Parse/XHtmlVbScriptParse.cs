using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript
{

    public record XHtmlVbScriptParse : VbScriptParse
    {
        public XmlElement       Element     { get; private set; }
        public XmlAttribute?    Attribute   { get; private set; }
        public XHtmlVbScriptParse(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, XmlElement element, XmlAttribute? attribute = null)
            : base(content, syntaxRoot)
        {
            this.Element   = element;
            this.Attribute = attribute;
        }
    }

}
