using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <ExprList>       ::=   <Expr> "," <ExprList>
     *                      | <Expr>
     * 
     * <IndexOrParams>  ::=   "(" <Expr> <CommaExprList> ")"
     *                      | "(" <CommaExprList> ")"
     *                      | "(" <Expr> ")"
     *                      | "(" ")"
     * 
     * <CommaExprList>  ::=   "," <Expr> <CommaExprList>
     *                      | "," <CommaExprList>
     *                      | "," <Expr>
     *                      | ","
     */

    public class ExpressionList : CodeNode, IReadOnlyList<Expression>
    {
        private IReadOnlyList<Expression> _Members;

        public ExpressionList(SyntaxNode source, IEnumerable<Expression> members)
            : base(source)
        {
            this._Members = members.ToArray();
        }

        public Expression this[Int32 index]
            => this._Members[index];

        public Int32 Count
            => this._Members.Count;

        public IEnumerator<Expression> GetEnumerator()
            => this._Members.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._Members.GetEnumerator();
    }

}
