using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript
{

    public class XslVbScriptParse : VbScriptParse
    {
        public Option<String> Prefix  { get; private set; }

        public XslVbScriptParse(VbScriptContent content, Option<String> prefix, Suspect<SyntaxNode> syntaxRoot)
            : base(content, syntaxRoot)
        {
            this.Prefix = prefix;
        }
    }

}
