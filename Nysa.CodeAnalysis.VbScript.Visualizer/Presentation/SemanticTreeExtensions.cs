using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public record ViewInfo(String Title, CodeNode Node, Func<IEnumerable<ViewInfo>> Children);

    public static class SemanticTreeExtensions
    {
        public static readonly Func<IEnumerable<ViewInfo>> _NoMore = () => None<ViewInfo>.Enumerable();

        public static ViewInfo ValueProperty(this CodeNode @this, String name, String? value, Func<IEnumerable<ViewInfo>>? children = null)
            => new ViewInfo(value == null ? name : String.Concat(name, " = ", value), @this, children ?? _NoMore);

        public static ViewInfo TypeProperty<T>(this CodeNode @this, String name, Func<IEnumerable<ViewInfo>>? children = null)
            where T : CodeNode
            => new ViewInfo(String.Concat(name, " : ", nameof(T)), @this, children ?? _NoMore);

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

        public static ViewInfo ToViewInfo(this LiteralValue @this, String? titlePrefix)
            => new ViewInfo(String.Concat(titlePrefix, nameof(LiteralValue)),
                            @this,
                            () => Return.Enumerable(@this.ValueProperty(nameof(LiteralValue.Type), @this.Type.ToString()),
                                                    @this.ValueProperty(nameof(LiteralValue.Value), @this.Value)));

        public static ViewInfo ToViewInfo(this Expression @this, String? titlePrefix)
            => @this switch
            {
                AccessExpression accessExpr => accessExpr.ToViewInfo(titlePrefix),
                ConstantOperation constOp => new ViewInfo(String.Concat(titlePrefix, nameof(ConstantOperation)),
                                                          constOp,
                                                          () => Return.Enumerable(constOp.ValueProperty(nameof(ConstantOperation.Type),
                                                                                                   constOp.Type.ToViewOp(),
                                                                                                   () => Return.Enumerable(constOp.Operand.ToViewInfo(""))))),
                LiteralValue literal => literal.ToViewInfo(titlePrefix),
                OperateExpression opExpr => new ViewInfo(String.Concat(titlePrefix, nameof(OperateExpression)),
                                                         opExpr,
                                                         () => Return.Enumerable(opExpr.ValueProperty(nameof(OperateExpression.Operation), opExpr.Operation.ToViewOp()),
                                                                                 opExpr.ValueProperty(nameof(OperateExpression.Operands), null, () => opExpr.Operands.Select(o => o.ToViewInfo(""))))),
                PrecedenceExpression precExpr => new ViewInfo(String.Concat(titlePrefix, nameof(PrecedenceExpression), " // '(' <expr> ')'"), precExpr, () => Return.Enumerable(precExpr.Value.ToViewInfo(""))),
                _ => throw new Exception("Program error.")
            };

        public static ViewInfo ToViewInfo(this PathExpressionItem @this)
            => @this switch
            {
                PathWith with => with.TypeProperty<PathWith>("'.'", _NoMore),
                PathIdentifier id => id.TypeProperty<PathIdentifier>(String.Concat("'", id.Value, "'"), _NoMore),
                PathValue value => value.TypeProperty<PathValue>("'(' <value> ')'", () => Return.Enumerable(value.Expression.ToViewInfo(""))),
                PathArguments args => args.HasPrecedence
                                      ? args.TypeProperty<PathArguments>("'(' <args> [', ' ...] ')'", () => args.Select(a => a.ToViewInfo("")))
                                      : args.TypeProperty<PathArguments>(" <args> [', ' ...]",  () => args.Select(a => a.ToViewInfo(""))),
                _ => throw new Exception("Program error.")
            };

        public static ViewInfo ToViewInfo(this PathExpression @this, String? titlePrefix)
            => new ViewInfo(String.Concat(titlePrefix, nameof(PathExpression)), @this, () => @this.Select(i => i.ToViewInfo()));

        public static ViewInfo ToViewInfo(this NewObjectExpression @this, String? titlePrefix)
            => new ViewInfo(String.Concat(titlePrefix, nameof(NewObjectExpression)), @this, () => Return.Enumerable(@this.Object.ToViewInfo("Class : ")));

        public static ViewInfo ToViewInfo(this AccessExpression @this, String? titlePrefix)
            => @this switch
            {
                NewObjectExpression newObjExpr => newObjExpr.ToViewInfo(titlePrefix),
                PathExpression pathExpr => pathExpr.ToViewInfo(titlePrefix),
                _ => throw new Exception("Program error.")
            };

        public static ViewInfo ToViewInfo(this AssignStatement @this)
            => new ViewInfo(nameof(AssignStatement),
                            @this,
                            () => Return.Enumerable(@this.Left.TypeProperty<AccessExpression>(nameof(AssignStatement.Left)),
                                                    @this.Right.TypeProperty<Expression>(nameof(AssignStatement.Right))));

        public static ViewInfo ToViewInfo(this CallStatement @this)
            => new ViewInfo(nameof(CallStatement),
                            @this,
                            () => Return.Enumerable(@this.AccessExpression.ToViewInfo("Call : ")));

        public static ViewInfo ToViewInfo(this Constant @this)
            => new ViewInfo(nameof(Constant),
                            @this,
                            () => @this.Expression.Match(e => Return.Enumerable(@this.ValueProperty(nameof(Constant.Name), @this.Name.Value),
                                                                                e.ToViewInfo(nameof(Expression))),
                                                         () => Return.Enumerable(@this.ValueProperty(nameof(Constant.Name), @this.Name.Value))));

        public static ViewInfo ToViewInfo(this ConstantDeclaration @this)
            => new ViewInfo(nameof(ConstantDeclaration),
                            @this,
                            () => @this.Constants.Select(c => c.ToViewInfo()));

        public static ViewInfo ToViewInfo(this ConstantStatement @this)
            => new ViewInfo(nameof(ConstantStatement), @this, () => @this.Constants.Select(c => c.ToViewInfo()));

        public static ViewInfo ToViewInfo(this DoLoopStatement @this)
            => new ViewInfo(nameof(DoLoopStatement),
                            @this,
                            () => @this.Statements.Select(s => s.ToViewInfo()));

        public static ViewInfo ToViewInfo(this DoLoopTestStatement @this)
            => new ViewInfo(nameof(DoLoopTestStatement),
                            @this,
                            () => Return.Enumerable(@this.TypeProperty<StatementList>(nameof(DoLoopTestStatement.Statements), () => @this.Statements.Select(s => s.ToViewInfo())),
                                                    @this.Condition.ToViewInfo(nameof(DoLoopTestStatement.Condition))));

        public static ViewInfo ToViewInfo(this DoTestLoopStatement @this)
            => new ViewInfo(nameof(DoTestLoopStatement),
                            @this,
                            () => Return.Enumerable(@this.Condition.ToViewInfo(nameof(DoTestLoopStatement.Condition)),
                                                    @this.TypeProperty<StatementList>(nameof(DoTestLoopStatement.Statements), () => @this.Statements.Select(s => s.ToViewInfo()))));

        public static ViewInfo ToViewInfo(this ElseIfBlock @this)
            => new ViewInfo(nameof(ElseIfBlock),
                            @this,
                            () => Return.Enumerable(@this.Predicate.ToViewInfo(nameof(ElseIfBlock.Predicate)),
                                                    @this.TypeProperty<StatementList>(nameof(ElseIfBlock.Statements), () => @this.Statements.Select(s => s.ToViewInfo()))));

        public static ViewInfo ToViewInfo(this FinalElseBlock @this)
            => new ViewInfo(nameof(FinalElseBlock),
                            @this,
                            () => Return.Enumerable(@this.TypeProperty<StatementList>(nameof(FinalElseBlock.Statements), () => @this.Statements.Select(s => s.ToViewInfo()))));

        public static ViewInfo ToViewInfo(this EraseStatement @this)
            => new ViewInfo(nameof(EraseStatement), @this, () => Return.Enumerable(@this.ValueProperty(nameof(EraseStatement.Name), @this.Name.Value)));

        public static ViewInfo ToViewInfo(this ExitStatement @this)
            => new ViewInfo(nameof(ExitStatement), @this, () => Return.Enumerable(@this.ValueProperty(nameof(ExitStatement.Type), @this.Type.ToString())));

        public static ViewInfo ToViewInfo(this Variable @this)
            => @this.ArrayRanks
                    .Match(r => @this.TypeProperty<Variable>(String.Concat("'", @this.Name, "'"),
                                                            () => Return.Enumerable(new ViewInfo(nameof(Variable.ArrayRanks), @this, () => r.Select(v => v.ToViewInfo(""))))),
                           () => @this.ValueProperty(nameof(Variable.Name), String.Concat("'", @this.Name, "'")));

        public static ViewInfo ToViewInfo(this FieldDeclaration @this)
            => new ViewInfo(nameof(FieldDeclaration),
                            @this,
                            () => Return.Enumerable(@this.ValueProperty(nameof(FieldDeclaration.Visibility), @this.Visibility.ToString()),
                                                    new ViewInfo(nameof(FieldDeclaration.Fields), @this, () => @this.Fields.Select(f => f.ToViewInfo()))));

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
                OnErrorGotoZero errGotoZero => errGotoZero.ToViewInfo(),
                OnErrorResumeNext errResumeNext => errResumeNext.ToViewInfo(),
                OptionExplicitStatement optExplicitStmt => optExplicitStmt.ToViewInfo(),
                RedimStatement redimStmt => redimStmt.ToViewInfo(),
                SelectCaseElse caseElse => caseElse.ToViewInfo(),
                SelectCaseWhen caseWhen => caseWhen.ToViewInfo(),
                SelectStatement selectStmt => selectStmt.ToViewInfo(),
                VariableDeclaration varDecl => varDecl.ToViewInfo(),
                WhileStatement whileStmt => whileStmt.ToViewInfo(),
                WithStatement withStmt => withStmt.ToViewInfo(),
                _ => throw new Exception("Program error.")
            };

        public static (String Title, IEnumerable<CodeNode> Children) ToViewInfo(this Program @this)
            => (nameof(Program), @this.Statements);

    }

}