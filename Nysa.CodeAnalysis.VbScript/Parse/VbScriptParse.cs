using System;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript
{

    public class VbScriptParse : Parse
    {
        public VbScriptContent      Content     { get; private set; }
        public Suspect<SyntaxNode>  SyntaxRoot  { get; private set; }

        public VbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot)
        {
            this.Content    = content;
            this.SyntaxRoot = syntaxRoot;
        }
    }

}