using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SyntaxNode = Nysa.Text.Parsing.Node;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public sealed record XslVbScriptParseScript : XslVbScriptParse
    {
        public Option<String>   Prefix      { get; private set; }
        public XmlElement       Element     { get; private set; }

        public XslVbScriptParseScript(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot, Option<String> prefix, XmlElement element)
            : base(content, syntaxRoot, semanticRoot)
        {
            this.Prefix     = prefix;
            this.Element    = element;
        }
    }

}
