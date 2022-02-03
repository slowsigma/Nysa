using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <WithStmt> ::= "With" <Expr> <NL> <BlockStmtList> "End" "With" <NL>
     */

    public class WithStatement : Statement, IReadOnlyList<Statement>
    {
        private IReadOnlyList<Statement> _Items;

        public Expression       Expression { get; private set; }

        public WithStatement(SyntaxNode source, Expression expression, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Expression = expression;
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
