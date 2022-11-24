using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (second half of definition)
     * <ForStmt> ::=   "For" <ExtendedID> "=" <Expr> "To" <Expr> <StepOpt> <NL> <BlockStmtList> "Next" <NL>
     *               | "For" "Each" <ExtendedID> "In" <Expr> <NL> <BlockStmtList> "Next" <NL>
     */

    public class ForEachStatement : Statement
    {
        public Identifier       Variable    { get; private set; }
        public Expression       In          { get; private set; }
        public StatementList    Statements  { get; private set; }

        public ForEachStatement(SyntaxNode source, Identifier variable, Expression @in, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Variable   = variable;
            this.In         = @in;
            this.Statements = new StatementList(source, statements);
        }
    }

}
