using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <SubDecl> ::=   <MethodAccessOpt> "Sub" <ExtendedID> <MethodArgList> <NL> <MethodStmtList> "End" "Sub" <NL>
     *               | <MethodAccessOpt> "Sub" <ExtendedID> <MethodArgList> <InlineStmt> "End" "Sub" <NL>
     */

    public class SubroutineDeclaration : MethodDeclaration
    {
        public SubroutineDeclaration(SyntaxNode source, Option<VisibilityTypes> visibility, Boolean isDefault, Identifier name, IEnumerable<ArgumentDefinition> arguments, IEnumerable<Statement> statements)
            : base(source, visibility, isDefault, name, arguments, statements)
        {
        }
    }

}
