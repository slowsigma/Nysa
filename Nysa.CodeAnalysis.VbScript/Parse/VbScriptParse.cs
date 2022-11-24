using System;

using SyntaxNode = Nysa.Text.Parsing.Node;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public record VbScriptParse : Parse
    {
        public VbScriptContent      Content         { get; private set; }
        public Suspect<SyntaxNode>  SyntaxRoot      { get; private set; }
        public Suspect<Program>     SemanticRoot    { get; private set; }

        public VbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot)
        {
            this.Content        = content;
            this.SyntaxRoot     = syntaxRoot;
            this.SemanticRoot   = semanticRoot;
        }
    }

}