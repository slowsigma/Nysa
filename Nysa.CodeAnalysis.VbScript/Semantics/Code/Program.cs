using System;
using System.Collections.Generic;
using System.Linq;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <Program>        ::= <NLOpt> <GlobalStmtList> {EOI}
     * <GlobalStmtList> ::=   <GlobalStmt> <GlobalStmtList>
     *                      | 
     * <GlobalStmt>     ::=   <OptionExplicit>
     *                      | <ClassDecl>
     *                      | <FieldDecl>
     *                      | <ConstDecl>
     *                      | <SubDecl>
     *                      | <FunctionDecl>
     *                      | <BlockStmt>
     */

    public class Program : CodeNode
    {
        public Boolean          OptionExplicit  { get; private set; }
        public StatementList    Statements      { get; private set; }

        public Program(SyntaxNode source, IEnumerable<Statement> statements)
            : base(source)
        {
            this.Statements     = new StatementList(source, statements);
            this.OptionExplicit = this.Statements.Any(n => n is OptionExplicitStatement);
        }
    }

}
