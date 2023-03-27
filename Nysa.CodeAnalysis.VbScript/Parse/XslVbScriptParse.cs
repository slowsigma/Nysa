using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SyntaxNode = Nysa.Text.Parsing.Node;

using Nysa.Logics;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public abstract record XslVbScriptParse(VbScriptSection content, Suspect<SyntaxNode> syntaxRoot, Suspect<Program> semanticRoot)
        : VbScriptParse(content, syntaxRoot, semanticRoot);

}
