using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript
{

    public class XslVbScriptParse : VbScriptParse
    {
        public Option<String>   Prefix      { get; private set; }
        public XmlElement       Element     { get; private set; }
        public XmlAttribute?    Attribute   { get; private set; }

        // When XslValueOf is not null, XNode is disconnected XText with placeholder name,
        // otherwise TextOrPlaceholder contains original XText or XElement of <xsl.text/>
        public IReadOnlyList<(XmlNode TextOrPlaceholder, XmlElement? XslValueOf)> Contents { get; private set; }

        public XslVbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot, Option<String> prefix, XmlElement element, XmlAttribute? attribute, IEnumerable<(XmlNode textOrPlaceholder, XmlElement? xslValueOf)>? contents)
            : base(content, syntaxRoot)
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
