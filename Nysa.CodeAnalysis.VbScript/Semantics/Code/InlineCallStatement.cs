using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <SubCallStmt> ::=   <QualifiedID> <SubSafeExprOpt> <CommaExprList>                                           // allows for: "SomeCall ex1, ex2, ex3" or "SomeCall"
     *                   | <QualifiedID> <SubSafeExprOpt>                                                           // allows for: "SomeCall" or "SomeCall ex1"
     *                   | <QualifiedID> "(" <Expr> ")" <CommaExprList>                                             // allows for: "SomeCall (ex1), ex2" or "SomeCall (ex1)"
     *                   | <QualifiedID> "(" <Expr> ")"                                                             // allows for: "SomeCall (ex1)"
     *                   | <QualifiedID> "(" ")"                                                                    // allows for: "SomeCall()"
     *                   | <QualifiedID> <IndexOrParamsList> "." <LeftExprTail> <SubSafeExprOpt> <CommaExprList>    // allows for: "SomeCall(ex1)(ex2).After ex3" or "SomeCall(ex1)(ex2).After ex3" or "SomeCall()().After"
     *                   | <QualifiedID> <IndexOrParamsListDot> <LeftExprTail> <SubSafeExprOpt> <CommaExprList>     // -same as previous-
     *                   | <QualifiedID> <IndexOrParamsList> "." <LeftExprTail> <SubSafeExprOpt>                    // allows for: "SomeCall(ex1)(ex2).After ex3" or "SomeCall(ex1)(ex2).After" or "SomeCall()().After ex1" or "SomeCall().After"
     *                   | <QualifiedID> <IndexOrParamsListDot> <LeftExprTail> <SubSafeExprOpt>                     // -same as previous-
     * 
     * <Expr>           ::= <ImpExpr>                       // down the expression rabbit hole
     * <SubSafeExprOpt> ::=   <SubSafeExpr>                 // down the expression rabbit hole
     *                      | 
     * 
     * <LeftExprTail>   ::=   <QualifiedIDTail> <IndexOrParamsList> "." <LeftExprTail>
     *                      | <QualifiedIDTail> <IndexOrParamsListDot> <LeftExprTail>
     *                      | <QualifiedIDTail> <IndexOrParamsList>
     *                      | <QualifiedIDTail>
     */

    // Note: In VbScript you must supply all the defined parameters.
    //       If a sub has no parameters it can be called with either:
    //          NoParamSub()          'or
    //          NoParamSub            'or
    //          Call NoParamSub()

    // Note: The AccessExpression of InlineCallStatement is always a PathExpression with two
    //       or more items where the final item is a PathArguments with HasPrecedence of
    //       false. Any prior PathArguments items will have HasPrecedence of true.


    public class InlineCallStatement : Statement
    {
        public AccessExpression AccessExpression { get; private set; }

        public InlineCallStatement(SyntaxNode source, AccessExpression accessExpression)
            : base(source)
        {
            this.AccessExpression = accessExpression;
        }
    }

}
