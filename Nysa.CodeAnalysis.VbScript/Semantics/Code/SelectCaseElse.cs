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
     * (second option)
     * <CaseStmtList> ::=   "Case" <ExprList> <NLOpt> <BlockStmtList> <CaseStmtList>
     *                    | "Case" "Else" <NLOpt> <BlockStmtList>
     *                    | 
     */

    public class SelectCaseElse : SelectCase
    {
        public SelectCaseElse(SyntaxNode source, IEnumerable<Statement> statements) : base(source, statements) { }
    }

}
