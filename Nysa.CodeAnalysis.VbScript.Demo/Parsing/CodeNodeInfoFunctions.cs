using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.Logics;
using Nysa.Text;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public static class CodeNodeInfoFunctions
    {
        private static readonly IReadOnlyList<CodeNodeInfo> _NoMembers = new List<CodeNodeInfo>();

        private static String ToViewOp(this OperationTypes @this)
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

        private static String ToViewOp(this ConstantOperationTypes @this)
            => @this switch
            {
                ConstantOperationTypes.add => "+",
                ConstantOperationTypes.precedence => "( ... )",
                ConstantOperationTypes.subtract => "-",
                _ => throw new Exception("Program error.")
            };

        private static IReadOnlyList<CodeNodeInfo> Members(params CodeNodeInfo[] infos)
            => infos.ToList();

        private static CodeNodeInfo ToProperty<T>(this T @this, String propertyName)
            => new CodeNodeInfo($"{propertyName} ({typeof(T).Name}) : {@this.ToString()}", null, _NoMembers, Option.None, Option.None);

        private static Option<Int32> FullLength(this Token? start, Token? end)
            => start.HasValue && end.HasValue
               ? ((end.Value.Span.Position - start.Value.Span.Position) + end.Value.Span.Length).Some()
               : Option<Int32>.None;

        private static CodeNodeInfo CreateWithBounds(this CodeNode @this, String title, IReadOnlyList<CodeNodeInfo?> members)
            => @this.TokenBounds()
                    .Make(bounds => new CodeNodeInfo(title,
                                                     @this,
                                                     members.Where(m => m != null).Cast<CodeNodeInfo>().ToList(),
                                                     bounds.Start.HasValue ? bounds.Start.Value.Span.Position.Some() : Option<Int32>.None,
                                                     bounds.Start.FullLength(bounds.End)));

        private static CodeNodeInfo CreateWithBounds(this CodeNode @this, String title, params CodeNodeInfo?[] members)
            => @this.TokenBounds()
                    .Make(bounds => new CodeNodeInfo(title,
                                                     @this,
                                                     members.Where(m => m != null).Cast<CodeNodeInfo>().ToList(),
                                                     bounds.Start.HasValue ? bounds.Start.Value.Span.Position.Some() : Option<Int32>.None,
                                                     bounds.Start.FullLength(bounds.End)));

        private static CodeNodeInfo ToProperty<T>(this Option<T> @this, String propertyName)
            => @this is Some<T> some
               ? some.Value.ToProperty(propertyName)
               : new CodeNodeInfo($"{propertyName} ({typeof(T).Name}) : {{not-specified}}", null, _NoMembers, Option.None, Option.None);

        private static CodeNodeInfo ToProperty<T>(this IReadOnlyList<T> @this, String propertyName, Func<T, CodeNodeInfo> itemTransform)
            where T : CodeNode
        {
            if (@this.Count > 0)
            {
                var firstBounds = @this.First().TokenBounds();
                var lastBounds = @this.Last().TokenBounds();

                var position = firstBounds.Start.HasValue ? firstBounds.Start.Value.Span.Position.Some() : Option<Int32>.None;
                var length   = firstBounds.Start.FullLength(lastBounds.End);

                return new CodeNodeInfo($"{propertyName} (IReadOnlyList<{typeof(T).Name}>)", null, @this.Select(c => itemTransform(c)).ToList(), position, length);
            }
            else
                return new CodeNodeInfo($"{propertyName} (IReadOnlyList<{typeof(T).Name}>)", null, _NoMembers, Option.None, Option.None);
        }

        private static CodeNodeInfo Property(this NewObjectExpression @this, String propertyName)
            => @this.CreateWithBounds($"{propertyName} ({nameof(NewObjectExpression)}) : ", @this.Object.Property(nameof(NewObjectExpression.Object)));

        private static CodeNodeInfo ToCodeNodeInfo(this PathExpressionItem @this)
            => @this switch
            {
                PathArguments pathArgs => pathArgs.CreateWithBounds(nameof(PathArguments),
                                                                    pathArgs.HasPrecedence.ToProperty(nameof(PathArguments.HasPrecedence)),
                                                                    new CodeNodeInfo(@"{items} (IReadOnlyList<Expression>)",
                                                                                     pathArgs,
                                                                                     pathArgs.Select(e => e.ToCodeNodeInfo()).ToList(),
                                                                                     Option.None,
                                                                                     Option.None)),
                PathIdentifier pathId => pathId.CreateWithBounds(nameof(PathIdentifier),
                                                                 pathId.Value.ToProperty(nameof(PathIdentifier.Value))),
                PathValue pathValue => pathValue.CreateWithBounds(nameof(PathValue),
                                                                  pathValue.Expression.ExprProperty(nameof(PathValue.Expression))),
                PathWith pathWith => pathWith.CreateWithBounds($". ({nameof(PathWith)})")
            };

        private static CodeNodeInfo Property(this PathExpression @this, String propertyName)
            => @this.CreateWithBounds($"{propertyName} ({nameof(PathExpression)}) : ", @this.Select(i => i.ToCodeNodeInfo()).ToList());



        private static CodeNodeInfo Property(this AccessExpression @this, String propertyName)
            => @this switch
            {
                NewObjectExpression newObjExpr => newObjExpr.Property(propertyName),
                PathExpression pathExpr => pathExpr.Property(propertyName)
            };

        private static CodeNodeInfo Property(this ConstantExpression @this, String propertyName)
            => @this switch
            {
                ConstantOperation constOp => constOp.CreateWithBounds($"{propertyName} ({nameof(ConstantOperation)}) : ",
                                                                      constOp.Type.ToProperty(nameof(ConstantOperationTypes)),
                                                                      constOp.Property(nameof(ConstantOperation.Operand))),
                LiteralValue literal => literal.CreateWithBounds($"{propertyName} ({nameof(LiteralValue)})",
                                                                 literal.Type.ToProperty(nameof(LiteralValue.Type)),
                                                                 literal.Value.ToProperty(nameof(LiteralValue.Value)))
            };

        private static CodeNodeInfo ToProperty(this Nysa.CodeAnalysis.VbScript.Semantics.Identifier @this, String propertyName)
            => @this.CreateWithBounds($"{propertyName} ({nameof(Nysa.CodeAnalysis.VbScript.Semantics.Identifier)}) : {@this.Value}");

        private static CodeNodeInfo ToCodeNodeInfo(this AccessExpression @this)
            => @this switch
            {
                NewObjectExpression newObjExpr => newObjExpr.CreateWithBounds(nameof(NewObjectExpression),
                                                                              newObjExpr.Object.Property(nameof(NewObjectExpression.Object))),
                PathExpression pathExpr => pathExpr.CreateWithBounds(nameof(PathExpression),
                                                                     pathExpr.Select(i => i.ToCodeNodeInfo()).ToList())
            };

        private static CodeNodeInfo ToCodeNodeInfo(this ConstantExpression @this)
            => @this switch
            {
                ConstantOperation constOp => constOp.CreateWithBounds(nameof(ConstantOperation),
                                                                      constOp.Type.ToProperty(nameof(ConstantOperationTypes)),
                                                                      constOp.Property(nameof(ConstantOperation.Operand))),
                LiteralValue literal => literal.CreateWithBounds(nameof(LiteralValue),
                                                                 literal.Type.ToProperty(nameof(LiteralValue.Type)),
                                                                 literal.Value.ToProperty(nameof(LiteralValue.Value)))
            };

        private static CodeNodeInfo ToCodeNodeInfo(this Expression @this)
            => @this switch
            {
                AccessExpression accessExpr => accessExpr.ToCodeNodeInfo(),
                ConstantExpression constExpr => constExpr.ToCodeNodeInfo(),
                OperateExpression opExpr => opExpr.CreateWithBounds(nameof(OperateExpression),
                                                                    opExpr.Operation.ToProperty(nameof(OperateExpression.Operation)),
                                                                    new CodeNodeInfo($"{nameof(OperateExpression.Operands)} (IReadOnlyList<Expression>) : ",
                                                                                     opExpr,
                                                                                     opExpr.Operands.Select(o => o.ToCodeNodeInfo()).ToList(),
                                                                                     Option.None,
                                                                                     Option.None)),
                PrecedenceExpression precExpr => precExpr.CreateWithBounds($"{nameof(PrecedenceExpression)}",
                                                                           precExpr.Value.ExprProperty(nameof(PrecedenceExpression.Value)))
            };

        private static CodeNodeInfo ExprProperty(this Expression @this, String propertyName)
            => @this switch
            {
                AccessExpression accessExpr => accessExpr.Property(propertyName),
                ConstantExpression constExpr => constExpr.Property(propertyName),
                OperateExpression opExpr => opExpr.CreateWithBounds($"{propertyName} ({nameof(OperateExpression)}) : ",
                                                                    opExpr.Operation.ToProperty(nameof(OperateExpression.Operation)),
                                                                    new CodeNodeInfo($"{nameof(OperateExpression.Operands)} (IReadOnlyList<Expression>) : ",
                                                                                     opExpr,
                                                                                     opExpr.Operands.Select(o => o.ToCodeNodeInfo()).ToList(),
                                                                                     Option.None,
                                                                                     Option.None)),
                PrecedenceExpression precExpr => precExpr.CreateWithBounds($"{propertyName} ({nameof(PrecedenceExpression)}) : ",
                                                                           precExpr.Value.ExprProperty(nameof(PrecedenceExpression.Value)))
            };

        private static CodeNodeInfo ToCodeNodeInfo(this MethodDeclaration @this)
            => @this switch
            {
                FunctionDeclaration funcDecl => funcDecl.CreateWithBounds(nameof(FunctionDeclaration),
                                                                          funcDecl.Visibility.Match(v => (CodeNodeInfo?)v.ToProperty(nameof(MethodDeclaration.Visibility)), () => null),
                                                                          funcDecl.IsDefault.ToProperty(nameof(MethodDeclaration.IsDefault)),
                                                                          funcDecl.Name.ToProperty(nameof(MethodDeclaration.Name)),
                                                                          funcDecl.Arguments.ToProperty(nameof(MethodDeclaration.Arguments), ConvertArgumentDefinition),
                                                                          funcDecl.Statements.ToProperty(nameof(MethodDeclaration.Statements), ToCodeNodeInfo)),
                PropertyDeclaration propDecl => propDecl.CreateWithBounds(nameof(PropertyDeclaration),
                                                                          propDecl.Access.ToProperty(nameof(PropertyDeclaration.Access)),
                                                                          propDecl.Visibility.Match(v => (CodeNodeInfo?)v.ToProperty(nameof(MethodDeclaration.Visibility)), () => null),
                                                                          propDecl.IsDefault.ToProperty(nameof(MethodDeclaration.IsDefault)),
                                                                          propDecl.Name.ToProperty(nameof(MethodDeclaration.Name)),
                                                                          propDecl.Arguments.ToProperty(nameof(MethodDeclaration.Arguments), ConvertArgumentDefinition),
                                                                          propDecl.Statements.ToProperty(nameof(MethodDeclaration.Statements), ToCodeNodeInfo)),
                SubroutineDeclaration subDecl => subDecl.CreateWithBounds(nameof(SubroutineDeclaration),
                                                                          subDecl.Visibility.Match(v => (CodeNodeInfo?)v.ToProperty(nameof(MethodDeclaration.Visibility)), () => null),
                                                                          subDecl.IsDefault.ToProperty(nameof(MethodDeclaration.IsDefault)),
                                                                          subDecl.Name.ToProperty(nameof(MethodDeclaration.Name)),
                                                                          subDecl.Arguments.ToProperty(nameof(MethodDeclaration.Arguments), ConvertArgumentDefinition),
                                                                          subDecl.Statements.ToProperty(nameof(MethodDeclaration.Statements), ToCodeNodeInfo))
            };

        private static CodeNodeInfo ToCodeNodeInfo(this AssignStatement @this)
            => @this.CreateWithBounds(nameof(AssignStatement),
                                      @this.Set.ToProperty(nameof(AssignStatement.Set)),
                                      @this.Left.ExprProperty(nameof(AssignStatement.Left)),
                                      @this.Right.ExprProperty(nameof(AssignStatement.Right)));

        private static CodeNodeInfo ToCodeNodeInfo(this Constant @this)
            => @this.CreateWithBounds(nameof(Constant),
                                      @this.Name.Value.ToProperty(nameof(Constant.Name)),
                                      @this.Expression.ToProperty(nameof(Constant.Expression)));

        private static CodeNodeInfo ConvertArrayRank(this LiteralValue @this)
            => @this.CreateWithBounds(nameof(LiteralValue),
                                      @this.Type.ToProperty(nameof(LiteralValue.Type)),
                                      @this.Value.ToProperty(nameof(LiteralValue.Value)));

        private static CodeNodeInfo ConvertVariable(this Variable @this)
            => @this.ArrayRanks
                    .Match(r => @this.CreateWithBounds(nameof(Variable),
                                                       @this.Name.Value.ToProperty(nameof(Variable.Name)),
                                                       r.ToProperty(nameof(Variable.ArrayRanks), ConvertArrayRank)),
                           () => @this.CreateWithBounds(nameof(Variable),
                                                        @this.Name.Value.ToProperty(nameof(Variable.Name))));

        private static CodeNodeInfo ConvertArgumentDefinition(this ArgumentDefinition @this)
            => @this.CreateWithBounds(nameof(ArgumentDefinition),
                                      @this.Modifier.ToProperty(nameof(ArgumentDefinition.Modifier)),
                                      @this.Name.ToProperty(nameof(ArgumentDefinition.Name)),
                                      @this.ArraySuffix.ToProperty(nameof(ArgumentDefinition.ArraySuffix)));

        private static CodeNodeInfo ConvertRedimVariable(this RedimVariable @this)
            => @this.CreateWithBounds(nameof(RedimVariable),
                                      @this.Name.ToProperty(nameof(RedimVariable.Name)),
                                      @this.RankExpressions.ToProperty(nameof(RedimVariable.RankExpressions), ToCodeNodeInfo));

        private static CodeNodeInfo ConvertSelectCase(this SelectCase @this)
            => @this switch
            {
                SelectCaseElse caseElse => caseElse.CreateWithBounds(nameof(SelectCaseElse),
                                                                     caseElse.Statements.ToProperty(nameof(SelectCase.Statements), ToCodeNodeInfo)),
                SelectCaseWhen caseWhen => caseWhen.CreateWithBounds(nameof(SelectCaseWhen),
                                                                     caseWhen.When.ToProperty(nameof(SelectCaseWhen.When), ToCodeNodeInfo),
                                                                     caseWhen.Statements.ToProperty(nameof(SelectCase.Statements), ToCodeNodeInfo))
            };

        private static CodeNodeInfo ConvertElseBlock(this ElseBlock @this)
            => @this switch
            {
                ElseIfBlock elseIf => elseIf.CreateWithBounds(nameof(ElseIfBlock),
                                                              elseIf.Predicate.ExprProperty(nameof(ElseIfBlock.Predicate)),
                                                              elseIf.Statements.ToProperty(nameof(ElseBlock.Statements), ToCodeNodeInfo)),
                FinalElseBlock finalElse => finalElse.CreateWithBounds(nameof(FinalElseBlock),
                                                                       finalElse.Statements.ToProperty(nameof(ElseBlock.Statements), ToCodeNodeInfo))
            };


        private static CodeNodeInfo ToCodeNodeInfo(this Statement @this)
            => @this switch
            {
                AssignStatement assnStmt => assnStmt.ToCodeNodeInfo(),
                CallStatement callStmt => callStmt.CreateWithBounds(nameof(CallStatement), Members(callStmt.AccessExpression.Property(nameof(CallStatement.AccessExpression)))),
                ClassDeclaration classDecl => classDecl.CreateWithBounds(nameof(ClassDeclaration),
                                                                         classDecl.Name.Value.ToProperty(nameof(ClassDeclaration.Name)),
                                                                         classDecl.Statements.ToProperty(nameof(ClassDeclaration.Statements), ToCodeNodeInfo)),
                ConstantDeclaration constDecl => constDecl.CreateWithBounds(nameof(ConstantDeclaration),
                                                                            constDecl.Visibility.ToProperty(nameof(ConstantDeclaration.Visibility)),
                                                                            new CodeNodeInfo($"{nameof(ConstantDeclaration.Constants)} (IReadOnlyList<Constant>) : ",
                                                                                             null,
                                                                                             constDecl.Constants.Select(c => c.ToCodeNodeInfo()).ToList(),
                                                                                             Option.None,
                                                                                             Option.None)),
                ConstantStatement constStmt => constStmt.CreateWithBounds(nameof(ConstantStatement),
                                                                          constStmt.Constants.Select(c => c.ToCodeNodeInfo()).ToList()),
                DoLoopStatement doLoopStmt => doLoopStmt.CreateWithBounds(nameof(DoLoopStatement),
                                                                          doLoopStmt.Statements.ToProperty(nameof(DoLoopStatement.Statements), ToCodeNodeInfo)),
                DoLoopTestStatement doLoopTestStmt => doLoopTestStmt.CreateWithBounds(nameof(DoLoopTestStatement),
                                                                                      doLoopTestStmt.Statements.ToProperty(nameof(DoLoopTestStatement.Statements), ToCodeNodeInfo),
                                                                                      doLoopTestStmt.Type.ToProperty(nameof(DoLoopTestStatement.Type)),
                                                                                      doLoopTestStmt.Condition.ExprProperty(nameof(DoLoopTestStatement.Condition))),
                DoTestLoopStatement doTestLoopStmt => doTestLoopStmt.CreateWithBounds(nameof(DoTestLoopStatement),
                                                                                      doTestLoopStmt.Type.ToProperty(nameof(DoTestLoopStatement.Type)),
                                                                                      doTestLoopStmt.Condition.ExprProperty(nameof(DoTestLoopStatement.Condition)),
                                                                                      doTestLoopStmt.Statements.ToProperty(nameof(DoTestLoopStatement.Statements), ToCodeNodeInfo)),
                EraseStatement eraseStmt => eraseStmt.CreateWithBounds(nameof(EraseStatement),
                                                                       eraseStmt.Name.Value.ToProperty(nameof(EraseStatement.Name))),
                ExitStatement exitStmt => exitStmt.CreateWithBounds(nameof(ExitStatement),
                                                                    exitStmt.Type.ToProperty(nameof(ExitStatement.Type))),
                FieldDeclaration fieldDecl => fieldDecl.CreateWithBounds(nameof(FieldDeclaration),
                                                                         fieldDecl.Visibility.ToProperty(nameof(FieldDeclaration.Visibility)),
                                                                         fieldDecl.Fields.ToProperty(nameof(FieldDeclaration.Fields), ConvertVariable)),
                ForEachStatement forEachStmt => forEachStmt.CreateWithBounds(nameof(ForEachStatement),
                                                                             forEachStmt.Variable.Value.ToProperty(nameof(ForEachStatement.Variable)),
                                                                             forEachStmt.In.ExprProperty(nameof(ForEachStatement.In)),
                                                                             forEachStmt.Statements.ToProperty(nameof(ForEachStatement.Statements), ToCodeNodeInfo)),
                ForStatement forStmt => forStmt.CreateWithBounds(nameof(ForStatement),
                                                                 forStmt.Variable.ToProperty(nameof(ForStatement.Variable)),
                                                                 forStmt.From.ExprProperty(nameof(ForStatement.From)),
                                                                 forStmt.To.ExprProperty(nameof(ForStatement.To)),
                                                                 forStmt.Step.ToProperty(nameof(ForStatement.Step)),
                                                                 forStmt.Statements.ToProperty(nameof(ForStatement.Statements), ToCodeNodeInfo)),
                IfStatement ifStmt => ifStmt.CreateWithBounds(nameof(IfStatement),
                                                              ifStmt.Predicate.ExprProperty(nameof(IfStatement.Predicate)),
                                                              ifStmt.Consequent.ToProperty(nameof(IfStatement.Consequent), ToCodeNodeInfo),
                                                              ifStmt.Alternatives.ToProperty(nameof(IfStatement.Alternatives), ConvertElseBlock)),
                InlineCallStatement inlineCallStmt => inlineCallStmt.CreateWithBounds(nameof(InlineCallStatement),
                                                                                      inlineCallStmt.AccessExpression.Property(nameof(InlineCallStatement.AccessExpression))),
                MethodDeclaration methodDecl => methodDecl.ToCodeNodeInfo(),
                OnErrorGotoZero onErrorGotoZero => onErrorGotoZero.CreateWithBounds(nameof(OnErrorGotoZero)),
                OnErrorResumeNext onErrorResumeNext => onErrorResumeNext.CreateWithBounds(nameof(OnErrorResumeNext)),
                OptionExplicitStatement optionExplicitStmt => optionExplicitStmt.CreateWithBounds(nameof(OptionExplicitStatement)),
                RedimStatement redimStmt => redimStmt.CreateWithBounds(nameof(RedimStatement),
                                                                       redimStmt.Preserve.ToProperty(nameof(RedimStatement.Preserve)),
                                                                       redimStmt.Variables.ToProperty(nameof(RedimStatement.Variables), ConvertRedimVariable)),
                SelectStatement selectStmt => selectStmt.CreateWithBounds(nameof(SelectStatement),
                                                                          selectStmt.Value.ExprProperty(nameof(SelectStatement.Value)),
                                                                          selectStmt.Cases.ToProperty(nameof(SelectStatement.Cases), ConvertSelectCase)),
                VariableDeclaration variableDecl => variableDecl.CreateWithBounds(nameof(VariableDeclaration),
                                                                                  variableDecl.Variables.ToProperty(nameof(VariableDeclaration.Variables), ConvertVariable)),
                WhileStatement whileStmt => whileStmt.CreateWithBounds(nameof(WhileStatement),
                                                                       whileStmt.Condition.ExprProperty(nameof(WhileStatement.Condition)),
                                                                       whileStmt.Statements.ToProperty(nameof(WhileStatement.Statements), ToCodeNodeInfo)),
                WithStatement withStmt => withStmt.CreateWithBounds(nameof(WithStatement),
                                                                    withStmt.Expression.ExprProperty(nameof(WithStatement.Expression)),
                                                                    withStmt.Statements.ToProperty(nameof(WithStatement.Statements), ToCodeNodeInfo))
            };

        private static IReadOnlyList<CodeNodeInfo> ToCodeNodeInfos(this StatementList @this)
            => @this.Select(c => c.ToCodeNodeInfo())
                    .ToList();

        public static CodeNodeInfo ToCodeNodeInfo(this Program @this)
            => new CodeNodeInfo(nameof(Program), @this,
                                Members(@this.OptionExplicit.ToProperty(nameof(Program.OptionExplicit)),
                                        new CodeNodeInfo($"{nameof(Program.Statements)} ({nameof(StatementList)}) : ",
                                                         @this.Statements,
                                                         @this.Statements.Select(s => s.ToCodeNodeInfo()).ToList(),
                                                         Option.None,
                                                         Option.None)),
                                Option.None,
                                Option.None);

    }

}