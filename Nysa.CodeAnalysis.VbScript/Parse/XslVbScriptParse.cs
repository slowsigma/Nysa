using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SyntaxNode = Dorata.Text.Parsing.Node;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public record XslVbScriptParse : VbScriptParse
    {
        public Option<String>   Prefix      { get; private set; }
        public XmlElement       Element     { get; private set; }
        public XmlAttribute?    Attribute   { get; private set; }

        // When XslValueOf is not null, XNode is disconnected XText with placeholder name,
        // otherwise TextOrPlaceholder contains original XText or XElement of <xsl.text/>
        public IReadOnlyList<(XmlNode TextOrPlaceholder, XmlElement? XslValueOf)> Contents { get; private set; }

        public XslVbScriptParse(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot, Option<String> prefix, XmlElement element, XmlAttribute? attribute, IEnumerable<(XmlNode textOrPlaceholder, XmlElement? xslValueOf)>? contents)
            : base(content, syntaxRoot, semanticRoot)
        {
            this.Prefix     = prefix;
            this.Element    = element;
            this.Attribute  = attribute;
            this.Contents   = contents == null
                              ? new (XmlNode, XmlElement?)[] { }
                              : contents.ToArray();
        }
    }

}
