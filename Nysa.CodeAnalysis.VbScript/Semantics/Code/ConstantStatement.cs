using System;
using System.Collections.Generic;
using System.Linq;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <BlockConstDecl> ::= "Const" <ConstList> <NL>
     */

    public class ConstantStatement : Statement
    {
        public IReadOnlyList<Constant> Constants { get; private set; }

        public ConstantStatement(SyntaxNode source, IEnumerable<Constant> constants)
            : base(source)
        {
            this.Constants = constants.ToArray();
        }
    }

}
