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
     * <CaseStmtList> ::=   "Case" <ExprList> <NLOpt> <BlockStmtList> <CaseStmtList>    // <<-- SelectCaseWhen.cs
     *                    | "Case" "Else" <NLOpt> <BlockStmtList>                       // <<-- SelectCaseElse.cs
     *                    | 
     */

    public abstract class SelectCase : Statement
    {
        public StatementList Statements { get; private set; }
        protected SelectCase(SyntaxNode source, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Statements = new StatementList(source, statements);
        }
    }

}
