using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <ClassDecl> ::= "Class" <ExtendedID> <NL> <MemberDeclList> "End" "Class" <NL>
     * 
     * <MemberDeclList> ::=   <MemberDecl> <MemberDeclList>
     *                      | 
     *                      
     * <MemberDecl> ::=   <FieldDecl>
     *                  | <VarDecl>
     *                  | <ConstDecl>
     *                  | <SubDecl>
     *                  | <FunctionDecl>
     *                  | <PropertyDecl>                     
     */

    public class ClassDeclaration : Statement
    {
        public Identifier       Name    { get; private set; }
        public StatementList    Members { get; private set; }

        public ClassDeclaration(SyntaxNode source, Identifier name, StatementList members)
            : base(source)
        {
            this.Name       = name;
            this.Members    = members;
        }
    }

}
