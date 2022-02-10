using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    public class HtmlVbScriptParse : VbScriptParse
    {
        public HtmlNode Node { get; private set; }
        public HtmlVbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot, HtmlNode node)
            : base(content, syntaxRoot)
        {
            this.Node = node;
        }
    }

}
