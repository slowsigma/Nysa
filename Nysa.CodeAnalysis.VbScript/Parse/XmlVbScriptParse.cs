using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SyntaxNode = Dorata.Text.Parsing.Node;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public record XmlVbScriptParse : VbScriptParse
    {
        public XmlElement       Element     { get; private set; }
        public XmlAttribute?    Attribute   { get; private set; }

        public XmlVbScriptParse(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot, XmlElement element, XmlAttribute? attribute)
            : base(content, syntaxRoot, semanticRoot)
        {
            this.Element    = element;
            this.Attribute  = attribute;
        }
    }

}
