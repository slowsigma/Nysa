using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <ElseStmtList> ::=   "ElseIf" <Expr> "Then" <NL> <BlockStmtList> <ElseStmtList>  // <<-- ElseIfBlock.cs
     *                    | "ElseIf" <Expr> "Then" <InlineStmt> <NL> <ElseStmtList>     // <<-- ElseIfBlock.cs
     *                    | "Else" <InlineStmt> <NL>                                    // <<-- FinalElseBlock.cs
     *                    | "Else" <NL> <BlockStmtList>                                 // <<-- FinalElseBlock.cs
     *                    | 
     */

    public abstract class ElseBlock : Statement
    {
        public StatementList Statements { get; private set; }

        protected ElseBlock(SyntaxNode source, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Statements = new StatementList(source, statements);
        }
    }

}
