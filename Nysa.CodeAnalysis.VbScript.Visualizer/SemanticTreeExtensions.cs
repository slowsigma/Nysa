using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public static class SemanticTreeExtensions
    {
        public static readonly Func<IEnumerable<ViewInfo>> _NoMore = () => None<ViewInfo>.Enumerable();

        private static IEnumerable<ViewInfo> NoNulls(params ViewInfo?[] props)
        {
            foreach (var prop in props)
                if (prop != null)
                    yield return prop;
        }

        private static Func<IEnumerable<ViewInfo>> Properties(params ViewInfo?[] props)
            => () => NoNulls(props);

        public static ViewInfo ValueProperty(this CodeNode @this, String name, String? value, Func<IEnumerable<ViewInfo>>? children = null)
            => new ViewInfo(value == null ? name : String.Concat(name, " = ", value), @this, children ?? _NoMore);

        public static ViewInfo ToViewInfo<T>(this T @this, Func<IEnumerable<ViewInfo>>? children = null, Boolean childrenHighlight = true)
            where T : CodeNode
            => new ViewInfo(@this.GetType().Name, @this, children ?? _NoMore, childrenHighlight);

        public static ViewInfo AsProperty(this ViewInfo @this, params String[] propertyName)
            => new ViewInfo(String.Concat(String.Join(String.Empty, propertyName), " : ", @this.Title), @this.Node, @this.Children, @this.ChildrenHighlight);

        public static String ToViewOp(this OperationTypes @this)
            => @this switch
            {
                OperationTypes.Implication => "Imp",
                OperationTypes.Equivalence => "Eqv",
                OperationTypes.ExclusiveOr => "Xor",
                OperationTypes.Or => "Or",
                OperationTypes.And => "And",
                OperationTypes.Not => "Not",
                OperationTypes.Is => "Is",
                OperationTypes.IsNot => "Is Not",
                OperationTypes.GreaterOrEqual => ">=",
                OperationTypes.LesserOrEqual => "<=",
                OperationTypes.Greater => ">",
                OperationTypes.Lesser => "<",
                OperationTypes.NotEqual => "<>",
                OperationTypes.Equal => "=",
                OperationTypes.Concatenate => "&",
                OperationTypes.Add => "+",
                OperationTypes.Subtract => "-",
                OperationTypes.Mod => "Mod",
                OperationTypes.IntDivide => @"\",
                OperationTypes.Multiply => "*",
                OperationTypes.Divide => "/",
                OperationTypes.Exponentiate => "^",
                OperationTypes.SignPositive => "+",
                OperationTypes.SignNegative => "-",
                _ => throw new Exception("Program error.")
            };

        public static String ToViewOp(this ConstantOperationTypes @this)
            => @this switch
            {
                ConstantOperationTypes.add => "+",
                ConstantOperationTypes.precedence => "( ... )",
                ConstantOperationTypes.subtract => "-",
                _ => throw new Exception("Program error.")
            };


        public static ViewInfo ToViewInfo(this LiteralValue @this)
            => new ViewInfo(nameof(LiteralValue),
                            @this,
                            Properties(@this.ValueProperty(nameof(LiteralValue.Type), @this.Type.ToString()),
                                       @this.ValueProperty(nameof(LiteralValue.Value), @this.Value)));


        public static ViewInfo ToViewInfo(this Expression @this)
            => @this switch
            {
                AccessExpression accessExpr => accessExpr.ToViewInfo(),
                ConstantOperation constOp => new ViewInfo(nameof(ConstantOperation),
                                                          constOp,
                                                          Properties(constOp.ValueProperty(nameof(ConstantOperation.Type),
                                                                                           constOp.Type.ToViewOp(),
                                                                                           () => Return.Enumerable(constOp.Operand.ToViewInfo())))),
                LiteralValue literal => literal.ToViewInfo(),
                OperateExpression opExpr => new ViewInfo(nameof(OperateExpression),
                                                         opExpr,
                                                         Properties(opExpr.ValueProperty(nameof(OperateExpression.Operation), opExpr.Operation.ToViewOp()),
                                                                    opExpr.ValueProperty(nameof(OperateExpression.Operands), null, () => opExpr.Operands.Select(o => o.ToViewInfo())))),
                PrecedenceExpression precExpr => new ViewInfo(String.Concat(nameof(PrecedenceExpression), " // '(' <expr> ')'"), precExpr, Properties(precExpr.Value.ToViewInfo())),
                _ => throw new Exception("Program error.")
            };

        public static ViewInfo ToViewInfo(this PathExpressionItem @this)
            => @this switch
            {
                PathWith with => with.ToViewInfo<PathWith>(_NoMore).AsProperty("'.'"),
                PathIdentifier id => id.ToViewInfo<PathIdentifier>(_NoMore).AsProperty("'", id.Value, "'"),
                PathValue value => value.ToViewInfo<PathValue>(Properties(value.Expression.ToViewInfo())).AsProperty("'(' <value> ')'"),
                PathArguments args => args.HasPrecedence
                                      ? args.ToViewInfo<PathArguments>(() => args.Select(a => a.ToViewInfo())).AsProperty("'(' <args> [', ' ...] ')'")
                                      : args.ToViewInfo<PathArguments>( () => args.Select(a => a.ToViewInfo())).AsProperty(" <args> [', ' ...]"),
                _ => throw new Exception("Program error.")
            };

        // ^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^
        // This whole titlePrefix thing was supposed to be a way to deal with the fact that some classes
        // are abstract and we don't want to use the abstract type name for the type of the property. In
        // addition, the subtypes will have different constituents.
        //
        // One answer to this problem is to have ToViewInfo functions for those specific subtypes and then
        // have a function like so: ViewInfo AsProperty(this ViewInfo @this, String name).  This function
        // would repackage a ViewInfo record that only has the subtype name for it's title (coming from
        // the call that obtained the correct subtype).  This would remove the titlePrefix being passed
        // downward in favor of fixing the ViewInfo record at the point it needs the property name.
        // ^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^v^

        public static ViewInfo ToViewInfo(this PathExpression @this)
            => @this.ToViewInfo(() => @this.Select(i => i.ToViewInfo()));

        public static ViewInfo ToViewInfo(this NewObjectExpression @this)
            => @this.ToViewInfo(Properties(@this.Object.ToViewInfo().AsProperty("Class")));

        public static ViewInfo ToViewInfo(this AccessExpression @this)
            => @this switch
            {
                NewObjectExpression newObjExpr => newObjExpr.ToViewInfo(),
                PathExpression pathExpr => pathExpr.ToViewInfo(),
                _ => throw new Exception("Program error.")
            };

        public static ViewInfo ToViewInfo(this Identifier @this)
            => new ViewInfo(@this.Value, @this, _NoMore);

        public static ViewInfo ToViewInfo(this AssignStatement @this)
            => new ViewInfo(nameof(AssignStatement),
                            @this,
                            Properties(@this.Left.ToViewInfo().AsProperty(nameof(AssignStatement.Left)),
                                       @this.Right.ToViewInfo().AsProperty(nameof(AssignStatement.Right))));

        public static ViewInfo ToViewInfo(this CallStatement @this)
            => @this.ToViewInfo(Properties(@this.AccessExpression.ToViewInfo().AsProperty("Call")));

        public static ViewInfo ToViewInfo(this ClassDeclaration @this)
            => @this.ToViewInfo(Properties(@this.ValueProperty(nameof(ClassDeclaration.Name), @this.Name.Value, _NoMore),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(ClassDeclaration.Statements))));

        public static ViewInfo ToViewInfo(this Constant @this)
            => @this.ToViewInfo(Properties(@this.ValueProperty(nameof(Constant.Name), @this.Name.Value),
                                          @this.Expression.Map(e => e.ToViewInfo().AsProperty("Value")).OrNull()));

        public static ViewInfo ToViewInfo(this ConstantDeclaration @this)
            => @this.ToViewInfo(() => @this.Constants.Select(c => c.ToViewInfo()));

        public static ViewInfo ToViewInfo(this ConstantStatement @this)
            => @this.ToViewInfo(() => @this.Constants.Select(c => c.ToViewInfo()));

        public static ViewInfo ToViewInfo(this DoLoopStatement @this)
            => @this.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo()));

        public static ViewInfo ToViewInfo(this DoLoopTestStatement @this)
            => @this.ToViewInfo(Properties(@this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(DoLoopTestStatement.Statements)),
                                          @this.Condition.ToViewInfo().AsProperty(nameof(DoLoopTestStatement.Condition))));

        public static ViewInfo ToViewInfo(this DoTestLoopStatement @this)
            => @this.ToViewInfo(Properties(@this.Condition.ToViewInfo().AsProperty(nameof(DoTestLoopStatement.Condition)),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(DoTestLoopStatement.Statements))));

        public static ViewInfo ToViewInfo(this ElseIfBlock @this)
            => @this.ToViewInfo(Properties(@this.Predicate.ToViewInfo().AsProperty(nameof(ElseIfBlock.Predicate)),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(ElseIfBlock.Statements))));

        public static ViewInfo ToViewInfo(this FinalElseBlock @this)
            => @this.ToViewInfo(Properties(@this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(FinalElseBlock.Statements))));

        public static ViewInfo ToViewInfo(this ElseBlock @this)
            => @this switch
            {
                ElseIfBlock elseIf => elseIf.ToViewInfo(),
                FinalElseBlock finalElse => finalElse.ToViewInfo(),
                _ => throw new Exception("Program error.")
            };

        public static ViewInfo ToViewInfo(this EraseStatement @this)
            => @this.ToViewInfo(Properties(@this.ValueProperty(nameof(EraseStatement.Name), @this.Name.Value)));

        public static ViewInfo ToViewInfo(this ExitStatement @this)
            => @this.ToViewInfo(Properties(@this.ValueProperty(nameof(ExitStatement.Type), @this.Type.ToString())));

        public static ViewInfo ToViewInfo(this Variable @this)
            => @this.ArrayRanks
                    .Match(r => @this.ToViewInfo(Properties(new ViewInfo(nameof(Variable.ArrayRanks), @this, () => r.Select(v => v.ToViewInfo()))))
                                     .AsProperty(String.Concat("'", @this.Name, "'")),
                           () => @this.ToViewInfo<Variable>().AsProperty(string.Concat("'", @this.Name, "'")));

        public static ViewInfo ToViewInfo(this FieldDeclaration @this)
            => @this.ToViewInfo(Properties(@this.ValueProperty(nameof(FieldDeclaration.Visibility), @this.Visibility.ToString()),
                                          new ViewInfo(nameof(FieldDeclaration.Fields), @this, () => @this.Fields.Select(f => f.ToViewInfo()))));

        public static ViewInfo ToViewInfo(this ForEachStatement @this)
            => @this.ToViewInfo(Properties(@this.ValueProperty(nameof(ForEachStatement.Variable), @this.Variable.Value),
                                          @this.In.ToViewInfo().AsProperty(nameof(ForEachStatement.In)),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(ForEachStatement.Statements))));

        public static ViewInfo ToViewInfo(this ForStatement @this)
            => @this.ToViewInfo(Properties(@this.ValueProperty(nameof(ForStatement.Variable), @this.Variable.Value),
                                          @this.From.ToViewInfo().AsProperty(nameof(ForStatement.From)),
                                          @this.To.ToViewInfo().AsProperty(nameof(ForStatement.To)),
                                          @this.Step.Map(s => s.ToViewInfo().AsProperty(nameof(ForStatement.Step))).OrNull(),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(ForStatement.Statements))));

        public static ViewInfo ToViewInfo(this IfStatement @this)
            => @this.ToViewInfo(() => Return.Enumerable(@this.Predicate.ToViewInfo().AsProperty(nameof(IfStatement.Predicate)),
                                                       @this.Consequent.ToViewInfo(() => @this.Consequent.Select(s => s.ToViewInfo())).AsProperty("IfTrue"))
                                           .Concat(@this.Alternatives.Select(a => a.ToViewInfo())));

        public static ViewInfo ToViewInfo(this InlineCallStatement @this)
            => @this.ToViewInfo(Properties(@this.AccessExpression.ToViewInfo().AsProperty(nameof(InlineCallStatement.AccessExpression))));

        public static ViewInfo ToViewInfo(this ArgumentDefinition @this)
            => new ViewInfo(@this.Name.Value, @this, _NoMore);

        public static ViewInfo ToViewInfo(this FunctionDeclaration @this)
            => @this.ToViewInfo(Properties(@this.Visibility.Map(v => @this.ValueProperty(nameof(MethodDeclaration.Visibility), v.ToString())).OrNull(),
                                          @this.IsDefault ? @this.ValueProperty(nameof(MethodDeclaration.IsDefault), @this.IsDefault.ToString()) : null,
                                          @this.ValueProperty(nameof(MethodDeclaration.Name), @this.Name.Value),
                                          new ViewInfo(nameof(MethodDeclaration.Arguments), @this, () => @this.Arguments.Select(a => a.ToViewInfo())),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(MethodDeclaration.Statements))));

        public static ViewInfo ToViewInfo(this PropertyDeclaration @this)
            => @this.ToViewInfo(Properties(@this.Visibility.Map(v => @this.ValueProperty(nameof(MethodDeclaration.Visibility), v.ToString())).OrNull(),
                                         @this.ValueProperty(nameof(PropertyDeclaration.Access), @this.Access.ToString()),
                                         @this.IsDefault ? @this.ValueProperty(nameof(MethodDeclaration.IsDefault), @this.IsDefault.ToString()) : null,
                                         @this.ValueProperty(nameof(MethodDeclaration.Name), @this.Name.Value),
                                         new ViewInfo(nameof(MethodDeclaration.Arguments), @this, () => @this.Arguments.Select(a => a.ToViewInfo())),
                                         @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(MethodDeclaration.Statements))));

        public static ViewInfo ToViewInfo(this SubroutineDeclaration @this)
            => @this.ToViewInfo(Properties(@this.Visibility.Map(v => @this.ValueProperty(nameof(MethodDeclaration.Visibility), v.ToString())).OrNull(),
                                          @this.IsDefault ? @this.ValueProperty(nameof(MethodDeclaration.IsDefault), @this.IsDefault.ToString()) : null,
                                          @this.Name.ToViewInfo().AsProperty(nameof(MethodDeclaration.Name)),
                                          new ViewInfo(nameof(MethodDeclaration.Arguments), @this, () => @this.Arguments.Select(a => a.ToViewInfo()), true),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(MethodDeclaration.Statements))));

        public static ViewInfo ToViewInfo(this RedimVariable @this)
            => new ViewInfo(String.Concat("'", @this.Name, "'"),
                            @this,
                            Properties(new ViewInfo("Ranks", @this, () => @this.RankExpressions.Select(r => r.ToViewInfo()))));

        public static ViewInfo ToViewInfo(this RedimStatement @this)
            => @this.ToViewInfo(Properties(@this.ValueProperty(nameof(RedimStatement.Preserve), @this.Preserve.ToString()),
                                          new ViewInfo(nameof(RedimStatement.Variables), @this, () => @this.Variables.Select(v => v.ToViewInfo()))));

        public static ViewInfo ToViewInfo(this SelectCaseElse @this)
            => @this.ToViewInfo(Properties(@this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(SelectCaseElse.Statements))));

        public static ViewInfo ToViewInfo(this SelectCaseWhen @this)
            => @this.ToViewInfo(Properties(@this.When.ToViewInfo(() => @this.When.Select(e => e.ToViewInfo())).AsProperty(),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty()));

        public static ViewInfo ToViewInfo(this SelectStatement @this)
            => @this.ToViewInfo(Properties(@this.Value.ToViewInfo().AsProperty(nameof(SelectStatement.Value)),
                                          new ViewInfo(nameof(SelectStatement.Cases), @this, () => @this.Cases.Select(c => c.ToViewInfo()))));

        public static ViewInfo ToViewInfo(this VariableDeclaration @this)
            => new ViewInfo(nameof(VariableDeclaration),
                            @this,
                            () => @this.Variables.Select(v => v.ToViewInfo()));

        public static ViewInfo ToViewInfo(this WhileStatement @this)
            => @this.ToViewInfo(Properties(@this.Condition.ToViewInfo().AsProperty(nameof(WhileStatement.Condition)),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(WhileStatement.Statements))));

        public static ViewInfo ToViewInfo(this WithStatement @this)
            => @this.ToViewInfo(Properties(@this.Expression.ToViewInfo().AsProperty(nameof(WithStatement.Expression)),
                                          @this.Statements.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo())).AsProperty(nameof(WithStatement.Statements))));

        public static ViewInfo ToViewInfo(this Statement @this)
            => @this switch
            {
                AssignStatement assignStmt => assignStmt.ToViewInfo(),
                CallStatement callStmt => callStmt.ToViewInfo(),
                ClassDeclaration classDecl => classDecl.ToViewInfo(),
                ConstantDeclaration constDecl => constDecl.ToViewInfo(),
                ConstantStatement constStmt => constStmt.ToViewInfo(),
                DoLoopStatement doLoopStmt => doLoopStmt.ToViewInfo(),
                DoLoopTestStatement loopTestStmt => loopTestStmt.ToViewInfo(),
                DoTestLoopStatement testLoopStmt => testLoopStmt.ToViewInfo(),
                ElseIfBlock elseIfBlock => elseIfBlock.ToViewInfo(),
                FinalElseBlock finalElseBlock => finalElseBlock.ToViewInfo(),
                EraseStatement eraseStmt => eraseStmt.ToViewInfo(),
                ExitStatement exitStmt => exitStmt.ToViewInfo(),
                FieldDeclaration fieldDecl => fieldDecl.ToViewInfo(),
                ForEachStatement forEachStmt => forEachStmt.ToViewInfo(),
                ForStatement forStmt => forStmt.ToViewInfo(),
                IfStatement ifStmt => ifStmt.ToViewInfo(),
                InlineCallStatement inlineCallStmt => inlineCallStmt.ToViewInfo(),
                FunctionDeclaration funcDecl => funcDecl.ToViewInfo(),
                PropertyDeclaration propDecl => propDecl.ToViewInfo(),
                SubroutineDeclaration subDecl => subDecl.ToViewInfo(),
                OnErrorGotoZero errGotoZero => new ViewInfo(nameof(OnErrorGotoZero), errGotoZero, _NoMore),
                OnErrorResumeNext errResumeNext => new ViewInfo(nameof(OnErrorResumeNext), errResumeNext, _NoMore),
                OptionExplicitStatement optExplicitStmt => new ViewInfo(nameof(OptionExplicitStatement), optExplicitStmt, _NoMore),
                RedimStatement redimStmt => redimStmt.ToViewInfo(),
                SelectCaseElse caseElse => caseElse.ToViewInfo(),
                SelectCaseWhen caseWhen => caseWhen.ToViewInfo(),
                SelectStatement selectStmt => selectStmt.ToViewInfo(),
                VariableDeclaration varDecl => varDecl.ToViewInfo(),
                WhileStatement whileStmt => whileStmt.ToViewInfo(),
                WithStatement withStmt => withStmt.ToViewInfo(),
                _ => throw new Exception("Program error.")
            };

        public static ViewInfo ToViewInfo(this Program @this)
            => @this.ToViewInfo(() => @this.Statements.Select(s => s.ToViewInfo()));

    }

}