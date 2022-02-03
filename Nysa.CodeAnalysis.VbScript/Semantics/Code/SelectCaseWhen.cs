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
     * (first option)
     * <CaseStmtList> ::=   "Case" <ExprList> <NLOpt> <BlockStmtList> <CaseStmtList>
     *                    | "Case" "Else" <NLOpt> <BlockStmtList>
     *                    | 
     */

    public class SelectCaseWhen : SelectCase
    {
        public ExpressionList   When       { get; private set; }

        public SelectCaseWhen(SyntaxNode source, ExpressionList when, IEnumerable<Statement> statements)
            : base(source, statements)
        {
            this.When = when;
        }
    }

}
