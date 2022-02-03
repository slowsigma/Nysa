using System;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * "Erase" <ExtendedID>
     */

    public class EraseStatement : Statement
    {
        public Identifier Name{ get; private set; }

        public EraseStatement(SyntaxNode source, Identifier name)
            : base(source)
        {
            this.Name = name;
        }
    }

}
