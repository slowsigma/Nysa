using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <FunctionDecl> ::=   <MethodAccessOpt> "Function" <ExtendedID> <MethodArgList> <NL> <MethodStmtList> "End" "Function" <NL>
     *                    | <MethodAccessOpt> "Function" <ExtendedID> <MethodArgList> <InlineStmt> "End" "Function" <NL>
     * 
     * -- Commonalities
     * <MethodAccessOpt>   ::= "Public" "Default" | <AccessModifierOpt>
     * <AccessModifierOpt> ::= "Public" | "Private" | 
     * <ExtendedID>        ::= <SafeKeywordID> | {ID}
     * <MethodArgList>     ::= "(" <ArgList> ")" | "(" ")" | 
     * <MethodStmtList>    ::= <MethodStmt> <MethodStmtList> | 
     */

    public class FunctionDeclaration : MethodDeclaration
    {
        public FunctionDeclaration(SyntaxNode source, Option<VisibilityTypes> visibility, Boolean isDefault, Identifier name, IEnumerable<ArgumentDefinition> arguments, IEnumerable<Statement> statements)
            : base(source, visibility, isDefault, name, arguments, statements)
        {
        }
    }

}
