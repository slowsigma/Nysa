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
        public HtmlNode Element { get; private set; }
        public HtmlVbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot, HtmlNode element)
            : base(content, syntaxRoot)
        {
            this.Element = element;
        }
    }

}
