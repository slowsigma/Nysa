using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SyntaxNode = Dorata.Text.Parsing.Node;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public record XHtmlVbScriptParse : VbScriptParse
    {
        public XmlElement       Element     { get; private set; }
        public XmlAttribute?    Attribute   { get; private set; }
        
        public XHtmlVbScriptParse(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot, XmlElement element, XmlAttribute? attribute = null)
            : base(content, syntaxRoot, semanticRoot)
        {
            this.Element   = element;
            this.Attribute = attribute;
        }
    }

}
