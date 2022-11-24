using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <ConstDecl> ::= <AccessModifierOpt> "Const" <ConstList> <NL>
     */

    public class ConstantDeclaration : Statement
    {
        public Option<VisibilityTypes>  Visibility  { get; private set; }
        public IReadOnlyList<Constant>  Constants   { get; private set; }

        public ConstantDeclaration(SyntaxNode source, Option<VisibilityTypes> visibility, IEnumerable<Constant> constants)
            : base(source)
        {
            this.Visibility = visibility;
            this.Constants  = constants.ToArray();
        }
    }

}
