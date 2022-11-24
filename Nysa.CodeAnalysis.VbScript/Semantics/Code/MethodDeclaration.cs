using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <FunctionDecl> ::=   <MethodAccessOpt> "Function" <ExtendedID> <MethodArgList> <NL> <MethodStmtList> "End" "Function" <NL>
     *                    | <MethodAccessOpt> "Function" <ExtendedID> <MethodArgList> <InlineStmt> "End" "Function" <NL>
     * <PropertyDecl> ::=   <MethodAccessOpt> "Property" <PropertyAccessType> <ExtendedID> <MethodArgList> <NL> <MethodStmtList> "End" "Property" <NL>
     * <SubDecl>      ::=   <MethodAccessOpt> "Sub" <ExtendedID> <MethodArgList> <NL> <MethodStmtList> "End" "Sub" <NL>
     *                    | <MethodAccessOpt> "Sub" <ExtendedID> <MethodArgList> <InlineStmt> "End" "Sub" <NL>
     * 
     * -- Commonalities
     * <MethodAccessOpt>   ::= "Public" "Default" | <AccessModifierOpt>
     * <AccessModifierOpt> ::= "Public" | "Private" | 
     * <ExtendedID>        ::= <SafeKeywordID> | {ID}
     * <MethodArgList>     ::= "(" <ArgList> ")" | "(" ")" | 
     * <MethodStmtList>    ::= <MethodStmt> <MethodStmtList> | 
     * -- Two out of three
     * <InlineStmt> ::= <AssignStmt> | <CallStmt> | <SubCallStmt> | <ErrorStmt> | <ExitStmt> | <InlineIfStmt> | "Erase" <ExtendedID>
     */

    public abstract class MethodDeclaration : Statement
    {
        public Option<VisibilityTypes>              Visibility  { get; private set; }
        public Boolean                              IsDefault   { get; private set; }
        public Identifier                           Name        { get; private set; }
        public IReadOnlyList<ArgumentDefinition>    Arguments   { get; private set; }
        public StatementList                        Statements  { get; private set; }

        protected MethodDeclaration(SyntaxNode source, Option<VisibilityTypes> visibility, Boolean isDefault, Identifier name, IEnumerable<ArgumentDefinition> arguments, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Visibility = visibility;
            this.IsDefault  = isDefault;
            this.Name       = name;
            this.Arguments  = arguments.ToArray();
            this.Statements = new StatementList(source, statements);
        }
    }

}
