using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

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
        public Identifier       Name        { get; private set; }
        public StatementList    Statements  { get; private set; }

        public ClassDeclaration(SyntaxNode source, Identifier name, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Name       = name;
            this.Statements = new StatementList(source, statements);
        }
    }

}
