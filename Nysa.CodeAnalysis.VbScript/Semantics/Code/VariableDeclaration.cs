using System;
using System.Collections.Generic;
using System.Linq;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <VarDecl>      ::= "Dim" <VarName> <OtherVarsOpt> <NL>
     * <OtherVarsOpt> ::=   "," <VarName> <OtherVarsOpt>
     *                    | 
     */

    public class VariableDeclaration : Statement
    {
        public IReadOnlyList<Variable> Variables { get; private set; }

        public VariableDeclaration(SyntaxNode source, IEnumerable<Variable> variables)
            : base(source)
        {
            this.Variables = variables.ToArray();
        }
    }

}
