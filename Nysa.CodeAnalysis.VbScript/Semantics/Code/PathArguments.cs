using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SyntaxToken = Dorata.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * (HasPrecedence = true)
     * <IndexOrParams> ::=   "(" <Expr> <CommaExprList> ")"
     *                     | "(" <CommaExprList> ")"
     *                     | "(" <Expr> ")"
     *                     | "(" ")"
     *                     
     * (HasPrecedence = false)
     * <CommaExprList> ::=   "," <Expr> <CommaExprList>
     *                     | "," <CommaExprList>
     *                     | "," <Expr>
     *                     | ","
     */

    // Note: A PathExpression can only have one PathArguments item where HasPrecedence is false.
    //       This represents the case for InlineCallStatement where it will always have one as
    //       the last item in the PathExpression.

    public class PathArguments : PathExpressionItem, IReadOnlyList<Expression>
    {
        public Boolean HasPrecedence { get; private set; } // i.e., is enclosed in parenthesis

        private IReadOnlyList<Expression> _Items;

        public PathArguments(SyntaxToken source, Boolean hasPrecedence, IEnumerable<Expression> items)
            : base(source)
        {
            this.HasPrecedence = hasPrecedence;
            this._Items        = items.ToArray();
        }

        public Expression this[Int32 index]
            => this._Items[index];

        public Int32 Count
            => this._Items.Count;

        public IEnumerator<Expression> GetEnumerator()
            => this._Items.Cast<Expression>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._Items.GetEnumerator();
    }

}
