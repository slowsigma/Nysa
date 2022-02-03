using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript
{

    public class HtmlVbScriptParse : VbScriptParse
    {
        public HtmlVbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot) : base(content, syntaxRoot) { }
    }

}
