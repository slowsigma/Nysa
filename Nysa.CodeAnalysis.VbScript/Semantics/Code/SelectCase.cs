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

    public abstract class SelectCase : CodeNode, IReadOnlyList<Statement>
    {
        private IReadOnlyList<Statement> _Items;

        protected SelectCase(SyntaxNode source, IEnumerable<Statement> statements)
            : base(source)
        {
            this._Items = statements.ToArray();
        }

        public Int32 Count => this._Items.Count;
        public Statement this[Int32 index] => this._Items[index];
        public IEnumerator<Statement> GetEnumerator()
            => _Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._Items.GetEnumerator();
    }

}
