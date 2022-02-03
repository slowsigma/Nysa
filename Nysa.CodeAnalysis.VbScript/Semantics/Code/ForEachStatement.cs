using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (second half of definition)
     * <ForStmt> ::=   "For" <ExtendedID> "=" <Expr> "To" <Expr> <StepOpt> <NL> <BlockStmtList> "Next" <NL>
     *               | "For" "Each" <ExtendedID> "In" <Expr> <NL> <BlockStmtList> "Next" <NL>
     */

    public class ForEachStatement : Statement, IReadOnlyList<Statement>
    {
        private IReadOnlyList<Statement> _Items;

        public Identifier       Variable    { get; private set; }
        public Expression       In          { get; private set; }

        public ForEachStatement(SyntaxNode source, Identifier variable, Expression @in, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Variable   = variable;
            this.In         = @in;
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
