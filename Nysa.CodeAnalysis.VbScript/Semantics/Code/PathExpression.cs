using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <SubCallStmt> ::=   <QualifiedID> <SubSafeExprOpt> <CommaExprList>
     *                   | <QualifiedID> <SubSafeExprOpt>
     *                   | <QualifiedID> "(" <Expr> ")"   <CommaExprList>
     *                   | <QualifiedID> "(" <Expr> ")"
     *                   | <QualifiedID> "(" ")"
     *                   | <QualifiedID> <IndexOrParamsList>    "." <LeftExprTail> <SubSafeExprOpt> <CommaExprList>
     *                   | <QualifiedID> <IndexOrParamsListDot>     <LeftExprTail> <SubSafeExprOpt> <CommaExprList>
     *                   | <QualifiedID> <IndexOrParamsList>    "." <LeftExprTail> <SubSafeExprOpt>
     *                   | <QualifiedID> <IndexOrParamsListDot>     <LeftExprTail> <SubSafeExprOpt>
     *
     * (fourth option only)
     * <Value> ::=   <ConstExpr>
     *             | <LeftExpr>
     *             | "(" <Expr> ")"
     *             | "(" <Expr> ")." <LeftExprTail>
     */

    public class PathExpression : AccessExpression, IReadOnlyList<PathExpressionItem>
    {
        private IReadOnlyList<PathExpressionItem> _Items;

        public PathExpression(SyntaxNode source, IEnumerable<PathExpressionItem> items)
            : base(source)
        {
            this._Items = items.ToArray();
        }

        public PathExpressionItem this[Int32 index]
            => this._Items[index];

        public Int32 Count
            => this._Items.Count;

        public IEnumerator<PathExpressionItem> GetEnumerator()
            => this._Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._Items.GetEnumerator();
    }

}
