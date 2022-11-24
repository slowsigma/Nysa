using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <WithStmt> ::= "With" <Expr> <NL> <BlockStmtList> "End" "With" <NL>
     */

    public class WithStatement : Statement
    {
        public Expression       Expression { get; private set; }
        public StatementList    Statements { get; private set; }

        public WithStatement(SyntaxNode source, Expression expression, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Expression = expression;
            this.Statements = new StatementList(source, statements);
        }
    }

}
