using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

using Nysa.Logics;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript
{

    public class XslVbScriptParse : VbScriptParse
    {
        public Option<String>   Prefix      { get; private set; }
        public XElement         Element     { get; private set; }
        public XAttribute?      Attribute   { get; private set; }

        public IReadOnlyList<(String Name, String Xml)> Substitutions { get; private set; }

        public XslVbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot, Option<String> prefix, XElement element, XAttribute? attribute, (String name, String xml)[]? substitutions)
            : base(content, syntaxRoot)
        {
            this.Prefix         = prefix;
            this.Element        = element;
            this.Attribute      = attribute;
            this.Substitutions  = substitutions ?? new (String Name, String Xml)[] { };
        }
    }

}
