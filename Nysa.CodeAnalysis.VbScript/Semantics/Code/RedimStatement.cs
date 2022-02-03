using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <RedimStmt> ::=   "Redim" <RedimDeclList> <NL>
     *                 | "Redim" "Preserve" <RedimDeclList> <NL>
     * 
     * <RedimDeclList> ::=   <RedimDecl> "," <RedimDeclList>
     *                     | <RedimDecl>
     */

    public class RedimStatement : Statement
    {
        public Boolean                      Preserve    { get; private set; }
        public IReadOnlyList<RedimVariable> Variables   { get; private set; }

        public RedimStatement(SyntaxNode source, Boolean preserve, IEnumerable<RedimVariable> variables)
            : base(source)
        {
            this.Preserve  = preserve;
            this.Variables = variables.ToArray();
        }
    }

}
