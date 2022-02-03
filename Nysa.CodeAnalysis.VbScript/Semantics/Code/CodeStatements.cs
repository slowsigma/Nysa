using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;
using Nysa.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    // Assists in code analysis by finding Statement nodes inside MethodDeclaration nodes
    // that match a specific type and predicate.
    public static class CodeStatements
    {

        private static Unit AddStatements<T>(this Statement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this switch 
            {
                T target when where(target) =>
                    statements.Affect(h => h.Add(target)),
                DoLoopStatement doLoopStmt =>
                    doLoopStmt.AddStatements(statements, where),
                DoLoopTestStatement doLoopTestStmt =>
                    doLoopTestStmt.AddStatements(statements,where),
                DoTestLoopStatement doTestLoopStmt =>
                    doTestLoopStmt.AddStatements(statements, where),
                ForEachStatement forEachStmt =>
                    forEachStmt.AddStatements(statements, where),
                ForStatement forStmt =>
                    forStmt.AddStatements(statements, where),
                IfStatement ifStmt =>
                    ifStmt.AddStatements(statements, where),
                SelectStatement selectStmt =>
                    selectStmt.AddStatements(statements, where),
                WhileStatement whileStmt =>
                    whileStmt.AddStatements(statements, where),
                WithStatement withStmt =>
                    withStmt.AddStatements(statements, where),
                _ => Unit.Value
            };

        private static Unit AddStatements<T>(this ElseBlock @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });

        private static Unit AddStatements<T>(this SelectCase @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });

        private static Unit AddStatements<T>(this DoLoopStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });
        private static Unit AddStatements<T>(this DoLoopTestStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });
        private static Unit AddStatements<T>(this DoTestLoopStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });
        private static Unit AddStatements<T>(this ForEachStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });
        private static Unit AddStatements<T>(this ForStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });
        private static Unit AddStatements<T>(this IfStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Consequent
                    .Affect(c => c.AddStatements(statements, where))
                    .Affect(u => @this.Alternatives.Affect(elses => { elses.AddStatements(statements, where); }));
        private static Unit AddStatements<T>(this SelectStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Cases.Affect(c => { c.AddStatements(statements, where); });
        private static Unit AddStatements<T>(this WhileStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });
        private static Unit AddStatements<T>(this WithStatement @this, HashSet<T> statements, Func<T, Boolean> where)
            => @this.Select(m => m).Affect(s => { s.AddStatements(statements, where); });

        public static HashSet<T> Flat<T>(this MethodDeclaration @this, Func<T, Boolean> where)
        {
            var result = new HashSet<T>();

            @this.Statements.Affect(s => { s.AddStatements(result, where); });

            return result;
        }

    }

}