using System;
using System.Collections.Generic;
using System.Text;

using SyntaxNode = Nysa.Text.Parsing.Node;

using HtmlAgilityPack;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public record HtmlVbScriptParse : VbScriptParse
    {
        public HtmlNode         Node        { get; private set; }
        public HtmlAttribute?   Attribute   { get; private set; }

        public HtmlVbScriptParse(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot, HtmlNode node, HtmlAttribute? attribute = null)
            : base(content, syntaxRoot, semanticRoot)
        {
            this.Node       = node;
            this.Attribute  = attribute;
        }
    }

}
