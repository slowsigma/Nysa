using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;
using Nysa.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    /// <summary>
    /// Functions for pulling all statements of a certain type and condition out of Program, Class, or Method nodes.
    /// </summary>
    public static class CodeStatements
    {

        private static Unit GatherStatements(this IEnumerable<Statement> @this, Func<Statement, Unit> collect)
        {
            foreach (var stmt in @this)
                stmt.GatherStatements(collect);

            return Unit.Value;
        }

        private static Unit GatherStatements(this Statement @this, Func<Statement, Unit> collect)
            => @this switch
            {
                AssignStatement assignStmt => collect(assignStmt),
                CallStatement callStmt => collect(callStmt),
                ClassDeclaration classDecl => collect(classDecl).Affect(u => { classDecl.Statements.GatherStatements(collect); }),
                ConstantDeclaration constDecl => collect(constDecl),
                ConstantStatement constStmt => collect(constStmt),
                DoLoopStatement doLoopStmt => collect(doLoopStmt).Affect(u => { doLoopStmt.Statements.Select(s => s).GatherStatements(collect); }),
                DoLoopTestStatement doLoopTestStmt => collect(doLoopTestStmt).Affect(u => { doLoopTestStmt.Statements.Select(s => s).GatherStatements(collect); }),
                DoTestLoopStatement doTestLoopStmt => collect(doTestLoopStmt).Affect(u => { doTestLoopStmt.Statements.Select(s => s).GatherStatements(collect); }),
                ElseBlock elseBlk => collect(elseBlk).Affect(u => { elseBlk.Statements.GatherStatements(collect); }),
                EraseStatement eraseStmt => collect(eraseStmt),
                ExitStatement exitStmt => collect(exitStmt),
                FieldDeclaration fieldDecl => collect(fieldDecl),
                ForEachStatement forEachStmt => collect(forEachStmt).Affect(u => { forEachStmt.Statements.Select(s => s).GatherStatements(collect); }),
                ForStatement forStmt => collect(forStmt).Affect(u => { forStmt.Statements.Select(s => s).GatherStatements(collect); }),
                FunctionDeclaration funcDecl => collect(funcDecl).Affect(u => { funcDecl.Statements.GatherStatements(collect); }),
                IfStatement ifStmt => collect(ifStmt).Affect(u => { ifStmt.Consequent.GatherStatements(collect); ifStmt.Alternatives.GatherStatements(collect); }),
                InlineCallStatement inlineStmt => collect(inlineStmt),
                OnErrorGotoZero onErrorZero => collect(onErrorZero),
                OnErrorResumeNext onErrorNext => collect(onErrorNext),
                OptionExplicitStatement optExpicitStmt => collect(optExpicitStmt),
                PropertyDeclaration propDecl => collect(propDecl).Affect(u => { propDecl.Statements.GatherStatements(collect); }),
                RedimStatement redimStmt => collect(redimStmt),
                SelectCase selectCase => collect(selectCase).Affect(u => { selectCase.Statements.GatherStatements(collect); }),
                SelectStatement selectStmt => collect(selectStmt).Affect(u => { selectStmt.Cases.GatherStatements(collect); }),
                SubroutineDeclaration subDecl => collect(subDecl).Affect(u => { subDecl.Statements.GatherStatements(collect); }),
                VariableDeclaration varDecl => collect(varDecl),
                WhileStatement whileStmt => collect(whileStmt).Affect(u => { whileStmt.Statements.GatherStatements(collect); }),
                WithStatement withStmt => collect(withStmt).Affect(u => { withStmt.Statements.GatherStatements(collect); }),
                _ => Unit.Value
            };

        private static Unit GatherPathExpressions(this Expression @this, Func<PathExpression, Unit> collect)
            => @this switch
            {
                PathExpression pathExpr => collect(pathExpr),
                OperateExpression opExpr => opExpr.Operands.Affect(o => { o.GatherPathExpressions(collect); }),
                PrecedenceExpression precExpr => precExpr.Value.GatherPathExpressions(collect),
                _ => Unit.Value
            };

        // Note: This method only gets the path expressions from a single statement. It does not
        //       attempt to find sub-statement expressions. Use the GatherStatements prior to
        //       calling this function in order to ensure sub-statements are searched.
        private static Unit GatherPathExpressions(this Statement @this, Func<PathExpression, Unit> collect)
            => @this switch
            {
                AssignStatement assignStmt => assignStmt.Affect(u =>
                                             {
                                                 assignStmt.Left.GatherPathExpressions(collect);
                                                 assignStmt.Right.GatherPathExpressions(collect);
                                             }),
                CallStatement callStmt => callStmt.AccessExpression.GatherPathExpressions(collect),
                DoLoopTestStatement loopTestStmt => loopTestStmt.Condition.GatherPathExpressions(collect),
                DoTestLoopStatement testLoopStmt => testLoopStmt.Condition.GatherPathExpressions(collect),
                ElseIfBlock elseIf => elseIf.Predicate.GatherPathExpressions(collect),
                ForEachStatement forEach => forEach.In.GatherPathExpressions(collect),
                ForStatement forStmt => forStmt.Affect(f =>
                                        {
                                            f.From.GatherPathExpressions(collect);
                                            f.To.GatherPathExpressions(collect);
                                            if (f.Step is Some<Expression> stepExpr)
                                                stepExpr.Value.GatherPathExpressions(collect);
                                        }),
                IfStatement ifStmt => ifStmt.Predicate.GatherPathExpressions(collect),
                InlineCallStatement inlineCallStmt => inlineCallStmt.AccessExpression.GatherPathExpressions(collect),
                RedimStatement redim => redim.Variables.SelectMany(v => v.RankExpressions).Affect(e => { e.GatherPathExpressions(collect); }),
                SelectCaseWhen caseWhen => caseWhen.When.Affect(e => { e.GatherPathExpressions(collect); }),
                SelectStatement selectStmt => selectStmt.Value.GatherPathExpressions(collect),
                WhileStatement whileStmt => whileStmt.Condition.GatherPathExpressions(collect),
                WithStatement withStmt => withStmt.Expression.GatherPathExpressions(collect),
                _ => Unit.Value
            };

        public static List<T> GetAll<T>(this Program @this, Func<T, Boolean> where)
            where T : Statement
        {
            var list = new List<T>();

            @this.Statements.GatherStatements(s => { if (s is T asT && where(asT)) { list.Add(asT); } return Unit.Value; });

            return list;
        }

        public static IReadOnlyList<Statement> GetAll(this ClassDeclaration @this, Func<Statement, Boolean> where)
        {
            var list = new List<Statement>();

            @this.Statements.GatherStatements(s => { if (where(s)) { list.Add(s); } return Unit.Value; });

            return list;
        }

        public static List<T> GetAll<T>(this MethodDeclaration @this, Func<T, Boolean> where)
            where T : Statement
        {
            var list = new List<T>();

            @this.GatherStatements(s => { if (s is T asT && where(asT)) { list.Add(asT); } return Unit.Value; });

            return list;
        }

        public static List<T> GetAll<T>(this IReadOnlyList<Statement> @this, Func<T, Boolean> where)
            where T : Statement
        {
            var list = new List<T>();

            foreach (var stmt in @this)
            {
                if (stmt is T asT && where(asT))
                    list.Add(asT);

                stmt.GatherStatements(s => { if (s is T sAsT && where(sAsT)) { list.Add(sAsT); } return Unit.Value; });
            }

            return list;
        }

        public static List<PathExpression> GetPathExprs(this MethodDeclaration @this)
        {
            var list = new List<PathExpression>();
            var stmts = @this.GetAll<Statement>(s => true);

            stmts.Select(s => s)
                 .Affect(v => { v.GatherPathExpressions(p => { list.Add(p); return Unit.Value; }); });

            return list;
        }

        public static List<PathExpression> GetPathExprs(this Program @this)
        {
            var list = new List<PathExpression>();
            var stmts = @this.GetAll<Statement>(s => true);

            stmts.Select(s => s)
                 .Affect(v => { v.GatherPathExpressions(p => { list.Add(p); return Unit.Value; }); });

            return list;
        }

/*
        // object - 5
        // path   - 4
        public static Int32 ApparentType(this Expression @this)
            => @this switch
            {
                AccessExpression access => access is NewObjectExpression ? 5 : 4,
                ConstantExpression constant = 
            };

        public static Int32 ApparentType(this ConstantExpression @this)
            => @this switch
            {
                ConstantOperation op => op.Type 
            };
*/
    }

}