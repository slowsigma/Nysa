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

    public class ForStatement : Statement, IReadOnlyList<Statement>
    {
        private IReadOnlyList<Statement> _Items;

        public Identifier           Variable   { get; private set; }
        public Expression           From       { get; private set; }
        public Expression           To         { get; private set; }
        public Option<Expression>   Step       { get; private set; }

        public ForStatement(SyntaxNode source, Identifier variable, Expression from, Expression to, Option<Expression> step, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Variable   = variable;
            this.From       = from;
            this.To         = to;
            this.Step       = step;
            this._Items     = statements.ToArray();
        }

        public Int32 Count => this._Items.Count;
        public Statement this[Int32 index] => this._Items[index];
        public IEnumerator<Statement> GetEnumerator()
            => _Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_Items).GetEnumerator();
    }

}
