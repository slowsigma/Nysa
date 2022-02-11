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
                SelectStatement selectStmt => collect(selectStmt).Affect(u => { selectStmt.Cases.GatherStatements(collect); }),
                SubroutineDeclaration subDecl => collect(subDecl).Affect(u => { subDecl.Statements.GatherStatements(collect); }),
                VariableDeclaration varDecl => collect(varDecl),
                WhileStatement whileStmt => collect(whileStmt).Affect(u => { whileStmt.Statements.GatherStatements(collect); }),
                WithStatement withStmt => collect(withStmt).Affect(u => { withStmt.Statements.GatherStatements(collect); }),
                _ => Unit.Value
            };

        public static HashSet<T> GetAll<T>(this Program @this)
        {
            var @set = new HashSet<T>();

            @this.Statements.GatherStatements(s => { if (s is T asT) { @set.Add(asT); } return Unit.Value; });

            return @set;
        }

        public static HashSet<T> GetAll<T>(this Program @this, Func<T, Boolean> where)
        {
            var @set = new HashSet<T>();

            @this.Statements.GatherStatements(s => { if (s is T asT && where(asT)) { @set.Add(asT); } return Unit.Value; });

            return @set;
        }

        public static HashSet<T> GetAll<T>(this ClassDeclaration @this)
        {
            var @set = new HashSet<T>();

            @this.GatherStatements(s => { if (s is T asT) { @set.Add(asT); } return Unit.Value; });

            return @set;
        }

        public static HashSet<T> GetAll<T>(this ClassDeclaration @this, Func<T, Boolean> where)
        {
            var @set = new HashSet<T>();

            @this.GatherStatements(s => { if (s is T asT && where(asT)) { @set.Add(asT); } return Unit.Value; });

            return @set;
        }

        public static HashSet<T> GetAll<T>(this MethodDeclaration @this)
        {
            var @set = new HashSet<T>();

            @this.GatherStatements(s => { if (s is T asT) { @set.Add(asT); } return Unit.Value; });

            return @set;
        }

        public static HashSet<T> GetAll<T>(this MethodDeclaration @this, Func<T, Boolean> where)
        {
            var @set = new HashSet<T>();

            @this.GatherStatements(s => { if (s is T asT && where(asT)) { @set.Add(asT); } return Unit.Value; });

            return @set;
        }

    }

}