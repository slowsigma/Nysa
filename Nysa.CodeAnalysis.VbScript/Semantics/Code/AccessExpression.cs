using System;
using System.Collections.Generic;
using System.Linq;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <LeftExpr>     ::=   <QualifiedID>   <IndexOrParamsList>    "." <LeftExprTail>
     *                    | <QualifiedID>   <IndexOrParamsListDot>     <LeftExprTail>
     *                    | <QualifiedID>   <IndexOrParamsList>
     *                    | <QualifiedID>
     *                    | <NewObjectExpr>
     *                    | <SafeKeywordID>
     * <LeftExprTail> ::=   <QualifiedIDTail> <IndexOrParamsList>    "." <LeftExprTail>
     *                    | <QualifiedIDTail> <IndexOrParamsListDot>     <LeftExprTail>
     *                    | <QualifiedIDTail> <IndexOrParamsList>
     *                    | <QualifiedIDTail>
     *                    
     * <QualifiedID>     ::=   {IDDot} <QualifiedIDTail>                        // <<-- AccessChain(AccessName, _
     *                       | {DotIDDot} <QualifiedIDTail>                     // <<-- AccessChain(AccessWith, AccessName, _
     *                       | {ID}                                             // <<-- AccessName
     *                       | {DotID}                                          // <<-- AccessChain(AccessWith + AccessName)
     * <QualifiedIDTail> ::=   {IDDot} <QualifiedIDTail>                        // <<-- _, AccessName, _)
     *                       | {ID}                                             // <<-- _, AccessName)
     *                       | <KeywordID>                                      // <<-- _, AccessName)
     *                       
     * <IndexOrParamsList>    ::=   <IndexOrParams> <IndexOrParamsList>         // <<-- (_)(_)
     *                            | <IndexOrParams>                             // <<-- (_)
     * <IndexOrParamsListDot> ::=   <IndexOrParams> <IndexOrParamsListDot>      // <<-- (_)(_).
     *                            | <IndexOrParamsDot>                          // <<-- (_).
     *                            
     * <IndexOrParams>        ::=   "(" <Expr> <CommaExprList> ")"
     *                            | "(" <CommaExprList> ")"
     *                            | "(" <Expr> ")"
     *                            | "(" ")"
     * <IndexOrParamsDot>     ::=   "(" <Expr> <CommaExprList> ")."
     *                            | "(" <CommaExprList> ")."
     *                            | "(" <Expr> ")."
     *                            | "(" ")."
     *                            
     * <CommaExprList>        ::=   "," <Expr> <CommaExprList>
     *                            | "," <CommaExprList>
     *                            | "," <Expr>
     *                            | ","
     *                            
     */

    public abstract class AccessExpression : Expression
    {
        protected AccessExpression(SyntaxNode source) : base(source) { }
    }

}
