using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (first half of definition)
     * <ForStmt> ::=   "For" <ExtendedID> "=" <Expr> "To" <Expr> <StepOpt> <NL> <BlockStmtList> "Next" <NL>
     *               | "For" "Each" <ExtendedID> "In" <Expr> <NL> <BlockStmtList> "Next" <NL>
     * 
     * <StepOpt> ::=   "Step" <Expr>
     *               | 
     */

    public class ForStatement : Statement
    {
        public Identifier           Variable   { get; private set; }
        public Expression           From       { get; private set; }
        public Expression           To         { get; private set; }
        public Option<Expression>   Step       { get; private set; }
        public StatementList        Statements { get; private set; }

        public ForStatement(SyntaxNode source, Identifier variable, Expression from, Expression to, Option<Expression> step, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Variable   = variable;
            this.From       = from;
            this.To         = to;
            this.Step       = step;
            this.Statements = new StatementList(source, statements);
        }
    }

}
