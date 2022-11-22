using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <IfStmt>         ::= "If" <Expr> "Then" <NL> <BlockStmtList> <ElseStmtList> "End" "If" <NL>
     * <InlineIfStmt>   ::= "If" <Expr> "Then" <InlineStmtList> <ElseOpt> <EndIfOpt>
     * 
     * <ElseStmtList>   ::=   "ElseIf" <Expr> "Then" <NL> <BlockStmtList> <ElseStmtList>
     *                      | "ElseIf" <Expr> "Then" <InlineStmt> <NL> <ElseStmtList>
     *                      | "Else" <InlineStmt> <NL>
     *                      | "Else" <NL> <BlockStmtList>
     *                      | 
     * <ElseOpt>        ::=   "Else" <InlineStmtList>
     *                      | 
     * <EndIfOpt>       ::=   "End" "If"
     *                      | 
     * <InlineStmtList> ::=   <InlineStmt> <InlineNL> <InlineStmtList>
     *                      | <InlineStmt> <InlineNL>
     *                      | <InlineStmt>
     */

    public class IfStatement : Statement
    {
        public Expression                   Predicate       { get; private set; }
        public StatementList                Consequent      { get; private set; }
        public IReadOnlyList<ElseBlock>     Alternatives    { get; private set; }

        public IfStatement(SyntaxNode source, Expression predicate, IEnumerable<Statement> consequent, IEnumerable<ElseIfBlock> elseIfs, Option<FinalElseBlock> @else)
            : base(source)
        {
            this.Predicate      = predicate;
            this.Consequent     = new StatementList(source, consequent);
            this.Alternatives   = @else.Match(e => elseIfs.Select(i => (ElseBlock)i).Concat(Return.Enumerable(e)),
                                              () => elseIfs.Select(i => (ElseBlock)i))
                                       .ToArray();
        }
    }

}
