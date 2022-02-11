using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (fourth option)
     * <LoopStmt> ::=   "Do" <LoopType> <Expr> <NL> <BlockStmtList> "Loop" <NL>
     *                | "Do" <NL> <BlockStmtList> "Loop" <LoopType> <Expr> <NL>
     *                | "Do" <NL> <BlockStmtList> "Loop" <NL>
     *                | "While" <Expr> <NL> <BlockStmtList> "WEnd" <NL>
     */

    public class WhileStatement : Statement
    {
        public Expression       Condition  { get; private set; }
        public StatementList    Statements { get; private set; }

        public WhileStatement(SyntaxNode source, Expression condition, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Condition  = condition;
            this.Statements = new StatementList(source, statements);
        }
    }

}
