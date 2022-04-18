using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    public record HtmlVbScriptParse : VbScriptParse
    {
        public HtmlNode         Node        { get; private set; }
        public HtmlAttribute?   Attribute   { get; private set; }

        public HtmlVbScriptParse(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, HtmlNode node, HtmlAttribute? attribute = null)
            : base(content, syntaxRoot)
        {
            this.Node       = node;
            this.Attribute  = attribute;
        }
    }

}
