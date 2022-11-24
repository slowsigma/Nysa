using System;
using System.Collections.Generic;
using System.Linq;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * -- GRAMMARS BELOW
     */

    public class OperateExpression : Expression
    {
        public OperationTypes               Operation   { get; private set; }
        public IReadOnlyList<Expression>    Operands    { get; private set; }

        public OperateExpression(SyntaxNode source, OperationTypes operation, IEnumerable<Expression> operands)
            : base(source)
        {
            this.Operation = operation;
            this.Operands  = operands.ToArray();
        }
    }

}

/*
    -- The expression rabbit hole...
    <Expr>              ::= <ImpExpr>
    <SubSafeExpr>       ::= <SubSafeImpExpr>

    <ImpExpr>           ::= <ImpExpr>        "Imp" <EqvExpr> | <EqvExpr>
    <SubSafeImpExpr>    ::= <SubSafeImpExpr> "Imp" <EqvExpr> | <SubSafeEqvExpr>

    <EqvExpr>           ::= <EqvExpr>        "Eqv" <XorExpr> | <XorExpr>
    <SubSafeEqvExpr>    ::= <SubSafeEqvExpr> "Eqv" <XorExpr> | <SubSafeXorExpr>

    <XorExpr>           ::= <XorExpr>        "Xor" <OrExpr> | <OrExpr>
    <SubSafeXorExpr>    ::= <SubSafeXorExpr> "Xor" <OrExpr> | <SubSafeOrExpr>

    <OrExpr>            ::= <OrExpr>        "Or" <AndExpr> | <AndExpr>
    <SubSafeOrExpr>     ::= <SubSafeOrExpr> "Or" <AndExpr> | <SubSafeAndExpr>

    <AndExpr>           ::= <AndExpr>        "And" <NotExpr> | <NotExpr>
    <SubSafeAndExpr>    ::= <SubSafeAndExpr> "And" <NotExpr> | <SubSafeNotExpr>

    <NotExpr>           ::= "Not" <NotExpr> | <CompareExpr>
    <SubSafeNotExpr>    ::= "Not" <NotExpr> | <SubSafeCompareExpr>              // <NotExpr> on the right side of "Not" token

    <CompareExpr>       ::=    <CompareExpr> "Is" <ConcatExpr>
                             | <CompareExpr> "Is" "Not" <ConcatExpr>
                             | <CompareExpr> ">=" <ConcatExpr>
                             | <CompareExpr> "=>" <ConcatExpr>
                             | <CompareExpr> "<=" <ConcatExpr>
                             | <CompareExpr> "=<" <ConcatExpr>
                             | <CompareExpr> ">" <ConcatExpr>
                             | <CompareExpr> "<" <ConcatExpr>
                             | <CompareExpr> "<>" <ConcatExpr>
                             | <CompareExpr> "=" <ConcatExpr>
                             | <ConcatExpr>
    <SubSafeCompareExpr> ::=   <SubSafeCompareExpr> "Is" <ConcatExpr>
                             | <SubSafeCompareExpr> "Is" "Not" <ConcatExpr>
                             | <SubSafeCompareExpr> ">=" <ConcatExpr>
                             | <SubSafeCompareExpr> "=>" <ConcatExpr>
                             | <SubSafeCompareExpr> "<=" <ConcatExpr>
                             | <SubSafeCompareExpr> "=<" <ConcatExpr>
                             | <SubSafeCompareExpr> ">" <ConcatExpr>
                             | <SubSafeCompareExpr> "<" <ConcatExpr>
                             | <SubSafeCompareExpr> "<>" <ConcatExpr>
                             | <SubSafeCompareExpr> "=" <ConcatExpr>
                             | <SubSafeConcatExpr>

    <ConcatExpr>        ::= <ConcatExpr>        "&" <AddExpr> | <AddExpr>
    <SubSafeConcatExpr> ::= <SubSafeConcatExpr> "&" <AddExpr> | <SubSafeAddExpr>

    <AddExpr>           ::= <AddExpr>        "+" <ModExpr> | <AddExpr>        "-" <ModExpr> | <ModExpr>
    <SubSafeAddExpr>    ::= <SubSafeAddExpr> "+" <ModExpr> | <SubSafeAddExpr> "-" <ModExpr> | <SubSafeModExpr>

    <ModExpr>           ::= <ModExpr>        "Mod" <IntDivExpr> | <IntDivExpr>
    <SubSafeModExpr>    ::= <SubSafeModExpr> "Mod" <IntDivExpr> | <SubSafeIntDivExpr>

    <IntDivExpr>        ::= <IntDivExpr>        "\" <MultExpr> | <MultExpr>
    <SubSafeIntDivExpr> ::= <SubSafeIntDivExpr> "\" <MultExpr> | <SubSafeMultExpr>

    <MultExpr>          ::= <MultExpr>        "*" <UnaryExpr> | <MultExpr>        "/" <UnaryExpr> | <UnaryExpr>
    <SubSafeMultExpr>   ::= <SubSafeMultExpr> "*" <UnaryExpr> | <SubSafeMultExpr> "/" <UnaryExpr> | <SubSafeUnaryExpr>

    <UnaryExpr>         ::= "-" <UnaryExpr> | "+" <UnaryExpr> | <ExpExpr>
    <SubSafeUnaryExpr>  ::= "-" <UnaryExpr> | "+" <UnaryExpr> | <SubSafeExpExpr>

    <ExpExpr>           ::= <Value>        "^" <ExpExpr> | <Value>
    <SubSafeExpExpr>    ::= <SubSafeValue> "^" <ExpExpr> | <SubSafeValue>

    <Value>             ::= <ConstExpr> | <LeftExpr> | "(" <Expr> ")" | "(" <Expr> ")." <LeftExprTail>
    <SubSafeValue>      ::= <ConstExpr> | <LeftExpr>

    <ConstExpr>         ::= <BoolLiteral> | <IntLiteral> | {FloatLiteral} | {StringLiteral} | {DateLiteral} | <Nothing>

    <BoolLiteral>       ::= "True" | "False"

    <Nothing>           ::= "Nothing" | "Null" | "Empty"

    <LeftExpr>          ::= <QualifiedID>     <IndexOrParamsList> "." <LeftExprTail> | <QualifiedID>     <IndexOrParamsListDot> <LeftExprTail> | <QualifiedID>     <IndexOrParamsList> | <QualifiedID>     | <NewObjectExpr> | <SafeKeywordID>
    <LeftExprTail>      ::= <QualifiedIDTail> <IndexOrParamsList> "." <LeftExprTail> | <QualifiedIDTail> <IndexOrParamsListDot> <LeftExprTail> | <QualifiedIDTail> <IndexOrParamsList> | <QualifiedIDTail>
 */
