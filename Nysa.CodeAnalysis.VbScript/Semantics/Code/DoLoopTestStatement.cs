using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (second option)
     * <LoopStmt> ::=   "Do" <LoopType> <Expr> <NL> <BlockStmtList> "Loop" <NL>
     *                | "Do" <NL> <BlockStmtList> "Loop" <LoopType> <Expr> <NL>
     *                | "Do" <NL> <BlockStmtList> "Loop" <NL>
     *                | "While" <Expr> <NL> <BlockStmtList> "WEnd" <NL>
     */

    public class DoLoopTestStatement : Statement, IReadOnlyList<Statement>
    {
        private IReadOnlyList<Statement> _Items;

        public LoopTypes        Type        { get; private set; }
        public Expression       Condition   { get; private set; }

        public DoLoopTestStatement(SyntaxNode source, IEnumerable<Statement> statements, LoopTypes type, Expression condition)
            : base(source)
        {
            this._Items     = statements.ToArray();
            this.Type       = type;
            this.Condition  = condition;
        }

        public Int32 Count => this._Items.Count;
        public Statement this[Int32 index] => this._Items[index];
        public IEnumerator<Statement> GetEnumerator()
            => _Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_Items).GetEnumerator();
    }

}
