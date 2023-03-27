using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SyntaxNode = Nysa.Text.Parsing.Node;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public sealed record XslVbScriptParseElement : XslVbScriptParse
    {
        public XmlElement       Element     { get; private set; }

        // When XslValueOf is not null, TextOrPlaceholder is disconnected XmlText with placeholder name,
        // otherwise TextOrPlaceholder contains either original XmlText or XmlElement of type 'xsl:text'
        public IReadOnlyList<(XmlNode TextOrPlaceholder, XmlElement? XslValueOf)> Contents { get; private set; }

        public XslVbScriptParseElement(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot, XmlElement element, IEnumerable<(XmlNode textOrPlaceholder, XmlElement? xslValueOf)>? contents)
            : base(content, syntaxRoot, semanticRoot)
        {
            this.Element    = element;
            this.Contents   = contents == null
                              ? new (XmlNode, XmlElement?)[] { }
                              : contents.ToArray();
        }
    }

}
