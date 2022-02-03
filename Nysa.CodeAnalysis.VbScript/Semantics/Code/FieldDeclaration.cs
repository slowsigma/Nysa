using System;
using System.Collections.Generic;
using System.Linq;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    /*
     * <FieldDecl> ::=   "Private" <FieldName> <OtherVarsOpt> <NL>
     *                 | "Public" <FieldName> <OtherVarsOpt> <NL>
     *                 
     * <OtherVarsOpt> ::=   "," <VarName> <OtherVarsOpt>
     *                    |                      
     */

    public class FieldDeclaration : Statement
    {
        public VisibilityTypes          Visibility  { get; private set; }
        public IReadOnlyList<Variable>  Fields      { get; private set; }

        public FieldDeclaration(SyntaxNode source, VisibilityTypes visibility, IEnumerable<Variable> fields)
            : base(source)
        {
            this.Visibility = visibility;
            this.Fields     = fields.ToArray();
        }
    }

}
