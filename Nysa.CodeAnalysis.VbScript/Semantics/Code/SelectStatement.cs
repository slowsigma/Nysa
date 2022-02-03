using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <SelectStmt> ::= "Select" "Case" <Expr> <NL> <CaseStmtList> "End" "Select" <NL>
     * 
     * <CaseStmtList> ::=   "Case" <ExprList> <NLOpt> <BlockStmtList> <CaseStmtList>
     *                    | "Case" "Else" <NLOpt> <BlockStmtList>
     *                    | 
     */

    public class SelectStatement : Statement
    {
        public Expression                   Value   { get; private set; }
        public IReadOnlyList<SelectCase>    Cases   { get; private set; }

        public SelectStatement(SyntaxNode source, Expression value, IEnumerable<SelectCaseWhen> cases, Option<SelectCaseElse> @else)
            : base(source)
        {
            this.Value = value;
            this.Cases = @else.Match(e => cases.Select(c => (SelectCase)c).Concat(Return.Enumerable(e)).ToArray(),
                                     () => cases.Select(c => (SelectCase)c))
                              .ToArray();
        }
    }

}
