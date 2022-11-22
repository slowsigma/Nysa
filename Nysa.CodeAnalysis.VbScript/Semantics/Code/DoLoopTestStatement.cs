using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (second option)
     * <LoopStmt> ::=   "Do" <LoopType> <Expr> <NL> <BlockStmtList> "Loop" <NL>
     *                | "Do" <NL> <BlockStmtList> "Loop" <LoopType> <Expr> <NL>
     *                | "Do" <NL> <BlockStmtList> "Loop" <NL>
     *                | "While" <Expr> <NL> <BlockStmtList> "WEnd" <NL>
     */

    public class DoLoopTestStatement : Statement
    {
        public StatementList    Statements  { get; private set; }
        public LoopTypes        Type        { get; private set; }
        public Expression       Condition   { get; private set; }

        public DoLoopTestStatement(SyntaxNode source, IEnumerable<Statement> statements, LoopTypes type, Expression condition)
            : base(source)
        {
            this.Statements = new StatementList(source, statements);
            this.Type       = type;
            this.Condition  = condition;
        }
    }

}
