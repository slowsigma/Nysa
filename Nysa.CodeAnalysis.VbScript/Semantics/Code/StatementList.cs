using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <BlockStmt> ::=   <VarDecl>
     *                 | <RedimStmt>
     *                 | <IfStmt>
     *                 | <WithStmt>
     *                 | <SelectStmt>
     *                 | <LoopStmt>
     *                 | <ForStmt>
     *                 | <BlockConstDecl>
     *                 | <InlineStmt> <NL>
     *         
     * <InlineStmt> ::=   <AssignStmt>
     *                  | <CallStmt>
     *                  | <SubCallStmt>
     *                  | <ErrorStmt>
     *                  | <ExitStmt>
     *                  | <InlineIfStmt>
     *                  | "Erase" <ExtendedID>
     */

    public class StatementList : CodeNode, IReadOnlyList<Statement>
    {
        private IReadOnlyList<Statement> _Statements;

        public StatementList(SyntaxNode source, IEnumerable<Statement> statements)
            : base(source)
        {
            this._Statements = statements.ToArray();
        }

        public Statement this[Int32 index]
            => this._Statements[index];

        public Int32 Count
            => this._Statements.Count;

        public IEnumerator<Statement> GetEnumerator()
            => this._Statements.Cast<Statement>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._Statements.GetEnumerator();
    }

}
