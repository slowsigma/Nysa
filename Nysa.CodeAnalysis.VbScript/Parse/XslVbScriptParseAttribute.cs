using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SyntaxNode = Nysa.Text.Parsing.Node;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public sealed record XslVbScriptParseAttribute : XslVbScriptParse
    {
        public Option<String>   Prefix      { get; private set; }
        public XmlElement       Element     { get; private set; }
        public XmlAttribute     Attribute   { get; private set; }

        public IReadOnlyList<(String Placeholder, String Original)> Placeholders { get; private set; }

        public XslVbScriptParseAttribute(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot, Option<String> prefix, XmlElement element, XmlAttribute attribute, IEnumerable<(String Placeholder, String Original)> placeholders)
            : base(content, syntaxRoot, semanticRoot)
        {
            this.Prefix         = prefix;
            this.Element        = element;
            this.Attribute      = attribute;
            this.Placeholders   = placeholders.ToArray();
        }
    }

}
