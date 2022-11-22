﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (first option)
     * <LoopStmt> ::=   "Do" <LoopType> <Expr> <NL> <BlockStmtList> "Loop" <NL>
     *                | "Do" <NL> <BlockStmtList> "Loop" <LoopType> <Expr> <NL>
     *                | "Do" <NL> <BlockStmtList> "Loop" <NL>
     *                | "While" <Expr> <NL> <BlockStmtList> "WEnd" <NL>
     */

    public class DoTestLoopStatement : Statement
    {
        public LoopTypes        Type        { get; private set; }
        public Expression       Condition   { get; private set; }
        public StatementList    Statements  { get; private set; }

        public DoTestLoopStatement(SyntaxNode source, LoopTypes type, Expression condition, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Type       = type;
            this.Condition  = condition;
            this.Statements = new StatementList(source, statements);
        }
    }

}
