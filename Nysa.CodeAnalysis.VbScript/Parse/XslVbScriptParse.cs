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

        public Boolean          TextWrapped { get; private set; } // true = all text is in xsl:text elements; false = contains no xsl:text elements
        public IReadOnlyList<(String Name, XElement XslValue)> Substitutions { get; private set; } // content means xsl:value elements in the original

        public XslVbScriptParse(VbScriptContent content, Suspect<SyntaxNode> syntaxRoot, Option<String> prefix, XElement element, XAttribute? attribute, Boolean textWrapped, IEnumerable<(String name, XElement xslValue)>? substitutions)
            : base(content, syntaxRoot)
        {
            this.Prefix         = prefix;
            this.Element        = element;
            this.Attribute      = attribute;
            this.TextWrapped    = textWrapped;
            this.Substitutions  = substitutions == null
                                  ? new (String Name, XElement XslValue)[] { }
                                  : substitutions.ToArray();
        }
    }

}
