using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript
{

    public record XmlVbScriptParse : VbScriptParse
    {
        public XmlElement       Element     { get; private set; }
        public XmlAttribute?    Attribute   { get; private set; }

        public XmlVbScriptParse(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, XmlElement element, XmlAttribute? attribute)
            : base(content, syntaxRoot)
        {
            this.Element    = element;
            this.Attribute  = attribute;
        }
    }

}
