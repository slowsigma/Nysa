using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using Nysa.CodeAnalysis.VbScript.Semantics;

namespace CodeAnalysis.VbScript.Rescript
{

    public static class VbScript
    {
        private static readonly String UNEXPECTED_TYPE = "Unexpected Type";
        private static readonly String INDENT = "  ";

        private static readonly Dictionary<OperationTypes, String> OpIndex = new Dictionary<OperationTypes, string>()
        {
            { OperationTypes.Implication, "Imp" },
            { OperationTypes.Equivalence, "Eqv" },
            { OperationTypes.ExclusiveOr, "Xor" },
            { OperationTypes.Or, "Or" },
            { OperationTypes.And, "And" },
            { OperationTypes.Not, "Not" },
            { OperationTypes.Is, "Is" },
            { OperationTypes.IsNot, "Is Not" },
            { OperationTypes.GreaterOrEqual, ">=" },
            { OperationTypes.LesserOrEqual, "<=" },
            { OperationTypes.Greater, ">" },
            { OperationTypes.Lesser, "<" },
            { OperationTypes.NotEqual, "<>" },
            { OperationTypes.Equal, "=" },
            { OperationTypes.Concatenate, "&" },
            { OperationTypes.Add, "+" },
            { OperationTypes.Subtract, "-" },
            { OperationTypes.Mod, "Mod" },
            { OperationTypes.IntDivide, @"\" },
            { OperationTypes.Multiply, "*" },
            { OperationTypes.Divide, "/" },
            { OperationTypes.Exponentiate, "^" }
        };

        private static Action<String> Indented(this Action<String> @this)
            => s => { @this(String.Concat(INDENT, s)); };

        public static String ToVbScript(this PathArguments @this)
            => String.Join(", ", @this.Select(expr => expr.ToVbScript()))
                     .Make(estr => @this.HasPrecedence ? String.Concat("(", estr, ")") : String.Concat(" ", estr));

        public static String ToVbScript(this NewObjectExpression @this)
            => String.Concat("New ", @this.Object.ToVbScript());

        public static String ToVbScript(this PathExpression @this)
        {
            var previous = (PathExpressionItem?)null;
            var builder = new StringBuilder();

            foreach (var item in @this)
            {
                var str = item switch
                {
                    PathWith with => ".",
                    PathIdentifier id when (previous is PathIdentifier) => String.Concat(".", id.Value),
                    PathIdentifier id when (previous is PathArguments args && args.HasPrecedence) => String.Concat(".", id.Value),
                    PathIdentifier id => id.Value,
                    PathArguments args => args.ToVbScript(),
                    PathValue value => String.Concat("(", value.Expression.ToVbScript(), ")"),
                    _ => throw new Exception(UNEXPECTED_TYPE)
                };

                builder.Append(str);
                previous = item;
            }

            return builder.ToString();
        }

        public static String ToVbScript(this AccessExpression @this)
            =>   @this is NewObjectExpression newObject ? newObject.ToVbScript()
               : @this is PathExpression path ? path.ToVbScript()
               : throw new Exception(UNEXPECTED_TYPE);

        public static String ToVbScript(this ConstantOperation @this)
            => @this.Type switch
            {
                ConstantOperationTypes.add => String.Concat("+", @this.Operand.ToVbScript()),
                ConstantOperationTypes.subtract => String.Concat("-", @this.Operand.ToVbScript()),
                ConstantOperationTypes.precedence => String.Concat("(", @this.Operand.ToVbScript(), ")"),
                { } => throw new Exception(UNEXPECTED_TYPE)
            };

        public static String ToVbScript(this LiteralValue @this)
            => @this.Value;

        public static String ToVbScript(this ConstantExpression @this)
            =>   @this is ConstantOperation operation ? operation.ToVbScript()
               : @this is LiteralValue literal ? literal.ToVbScript()
               : throw new Exception(UNEXPECTED_TYPE);

        public static String ToVbScript(this OperateExpression @this)
            => @this.Operation switch
            {
                OperationTypes.SignNegative => String.Concat("-", @this.Operands[0]),
                OperationTypes.SignPositive => String.Concat("+", @this.Operands[0]),
                OperationTypes.Not when (@this.Operands.Count != 1) => throw new Exception(UNEXPECTED_TYPE),
                OperationTypes.Not => String.Concat("Not", " ", @this.Operands[0].ToVbScript()),
                { } => String.Join(String.Concat(" ", OpIndex[@this.Operation], " "), @this.Operands.Select(o => o.ToVbScript()))
            };

        public static String ToVbScript(this PrecedenceExpression @this)
            => String.Concat("(", @this.Value.ToVbScript(), ")");

        public static String ToVbScript(this Expression @this)
            =>   @this is AccessExpression access ? access.ToVbScript()
               : @this is ConstantExpression constant ? constant.ToVbScript()
               : @this is OperateExpression operate ? operate.ToVbScript()
               : @this is PrecedenceExpression precedence ? precedence.ToVbScript()
               : throw new Exception(UNEXPECTED_TYPE);

        public static String ToVbScript(this VisibilityTypes @this)
            => @this == VisibilityTypes.Private ? "Private" : "Public";

        public static String ToVbScript(this Option<VisibilityTypes> @this)
            => @this.Match(v => String.Concat(v.ToVbScript(), " "),
                           () => String.Empty);

        public static String ToVbScript(this Constant @this)
            => @this.Expression
                    .Match(e => String.Concat(@this.Name.Value, " ", "=", " ", e.ToVbScript()),
                           () => @this.Name.Value);

        public static String ToVbScript(this Variable @this)
            => @this.ArrayRanks
                    .Match(r => String.Concat(@this.Name.Value, "(", String.Join(", ", r.Select(v => v.ToVbScript())), ")"),
                           () => @this.Name.Value);

        public static String ToVbScript(this Option<ArgumentModifiers> @this)
            => @this.Match(m => m == ArgumentModifiers.ByRef ? "ByRef " : "ByVal ",
                           () => String.Empty);

        public static String ToVbScript(this ArgumentDefinition @this)
            => String.Concat(@this.Modifier.ToVbScript(), @this.Name.Value, @this.ArraySuffix ? "()" : String.Empty);

        public static String ToVbScript(this PropertyAccessTypes @this)
            => @this switch
            {
                PropertyAccessTypes.Get => "Get",
                PropertyAccessTypes.Let => "Let",
                PropertyAccessTypes.Set => "Set",
                { } => throw new Exception(UNEXPECTED_TYPE)
            };

        public static String ToVbScript(this RedimVariable @this)
            => String.Concat(@this.Name.Value, "(", String.Join(", ", @this.RankExpressions.Select(e => e.ToVbScript())), ")");

        public static Unit ToVbScript(this AssignStatement @this, Action<String> appendLine)
            => String.Concat(@this.Left.ToVbScript(), " ", "=", " ", @this.Right.ToVbScript())
                     .Affect(assign =>
                     {
                         if (@this.Set)
                             appendLine(String.Concat("Set", " ", assign));
                         else
                             appendLine(assign);
                     });

        public static Unit ToVbScript(this CallStatement @this, Action<String> appendLine)
            => String.Concat("Call", " ", @this.AccessExpression.ToVbScript())
                     .Affect(s => { appendLine(s); });

        public static Unit ToVbScript(this ClassDeclaration @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine(String.Concat("Class", " ", @this.Name.Value));

            foreach (var item in @this.Members)
            {
                item.ToVbScript(indented);
            }

            appendLine(String.Concat("End", " ", "Class"));
            appendLine(String.Empty);

            return Unit.Value;
        }

        public static Unit ToVbScript(this ConstantDeclaration @this, Action<String> appendLine)
            => String.Concat(@this.Visibility.ToVbScript(), "Const", " ", String.Join(", ", @this.Constants.Select(c => c.ToVbScript())))
                     .Affect(s =>
                     {
                         appendLine(s);

                         if (@this.Visibility is Some<VisibilityTypes> visType && visType.Value == VisibilityTypes.Public)
                             appendLine(String.Empty);
                     });

        public static Unit ToVbScript(this ConstantStatement @this, Action<String> appendLine)
            => String.Concat("Const", " ", String.Join(", ", @this.Constants.Select(c => c.ToVbScript())))
                     .Affect(s => { appendLine(s); });

        public static Unit ToVbScript(this DoLoopStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine("Do");

            foreach (var stmt in @this)
                stmt.ToVbScript(indented);

            appendLine("Loop");

            return Unit.Value;
        }

        public static Unit ToVbScript(this DoLoopTestStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine("Do");

            foreach (var stmt in @this)
                stmt.ToVbScript(indented);

            appendLine(String.Concat("Loop", " ", @this.Type == LoopTypes.Until ? "Until" : "While", " ", @this.Condition.ToVbScript()));

            return Unit.Value;
        }

        public static Unit ToVbScript(this DoTestLoopStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine(String.Concat("Do", " ", @this.Type == LoopTypes.Until ? "Until" : "While", " ", @this.Condition.ToVbScript()));

            foreach (var stmt in @this)
                stmt.ToVbScript(indented);

            appendLine("Loop");

            return Unit.Value;
        }

        public static Unit ToVbScript(this EraseStatement @this, Action<String> appendLine)
            => String.Concat("Erase", " ", @this.Name.Value)
                     .Affect(s => { appendLine(s); });

        public static Unit ToVbScript(this ExitStatement @this, Action<String> appendLine)
            => (@this.Type switch
            {
                ExitTypes.Do => "Do",
                ExitTypes.For => "For",
                ExitTypes.Function => "Function",
                ExitTypes.Property => "Property",
                ExitTypes.Sub => "Sub",
                { } => throw new Exception(UNEXPECTED_TYPE)
            }).Affect(type => { appendLine(String.Concat("Exit", " ", type)); });

        public static Unit ToVbScript(this FieldDeclaration @this, Action<String> appendLine)
            => String.Concat(@this.Visibility.ToVbScript(), " ")
                     .Affect(pfx =>
                     {
                         appendLine(String.Concat(pfx, String.Join(", ", @this.Fields.Select(v => v.ToVbScript()))));

                         if (@this.Visibility == VisibilityTypes.Public)
                             appendLine(String.Empty);
                     });

        public static Unit ToVbScript(this ForEachStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine(String.Concat("For", " ", "Each", " ", @this.Variable.Value, " ", "In", " ", @this.In.ToVbScript()));

            foreach (var stmt in @this)
                stmt.ToVbScript(indented);

            appendLine("Next");

            return Unit.Value;
        }

        public static Unit ToVbScript(this ForStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            var step = @this.Step.Match(e => String.Concat(" ", "Step", e.ToVbScript()), () => String.Empty);

            appendLine(String.Concat("For", " ", @this.Variable.Value, " ", "=", " ", @this.From.ToVbScript(), " ", "To", " ", @this.To.ToVbScript(), step));

            foreach (var stmt in @this)
                stmt.ToVbScript(indented);

            appendLine("Next");

            return Unit.Value;
        }

        public static Unit ToVbScript(this IfStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine(String.Concat("If", " ", @this.Predicate.ToVbScript(), " ", "Then"));

            foreach (var stmt in @this.Consequent)
                stmt.ToVbScript(indented);

            foreach (var alternative in @this.Alternatives)
            {
                appendLine(alternative switch
                {
                    ElseIfBlock elseIf => String.Concat("ElseIf", " ", elseIf.Predicate.ToVbScript(), " ", "Then"),
                    FinalElseBlock final => "Else",
                    _ =>  throw new Exception(UNEXPECTED_TYPE)
                });

                foreach (var stmt in alternative)
                    stmt.ToVbScript(indented);
            }

            appendLine(String.Concat("End", " ", "If"));
            return Unit.Value;
        }

        public static Unit ToVbScript(this InlineCallStatement @this, Action<String> appendLine)
            => @this.AccessExpression
                    .ToVbScript()
                    .Affect(e => { appendLine(e); });

        public static Unit ToVbScript(this MethodDeclaration @this, Action<String> appendLine, String methodTypeName)
        {
            var indented = appendLine.Indented();

            var access = @this.IsDefault ? String.Concat(@this.Visibility.ToVbScript(), "Default ")
                                         : @this.Visibility.ToVbScript();
            var args = String.Concat("(", String.Join(", ", @this.Arguments.Select(a => a.ToVbScript())), ")");

            appendLine(String.Concat(access, methodTypeName, " ", @this.Name.Value, args));

            foreach (var stmt in @this.Statements)
                stmt.ToVbScript(indented);

            appendLine(String.Concat("End", " ", methodTypeName));
            appendLine(String.Empty);

            return Unit.Value;
        }

        public static Unit ToVbScript(this MethodDeclaration @this, Action<String> appendLine)
            =>   @this is FunctionDeclaration   ? @this.ToVbScript(appendLine, "Function")
               : @this is PropertyDeclaration   ? @this.ToVbScript(appendLine, "Property")
               : @this is SubroutineDeclaration ? @this.ToVbScript(appendLine, "Sub")
               : throw new Exception(UNEXPECTED_TYPE);

        public static Unit ToVbScript(this OnErrorGotoZero @this, Action<String> appendLine)
            => String.Concat("On", " ", "Error", " ", "GoTo", " " , "0")
                     .Affect(s => { appendLine(s); });

        public static Unit ToVbScript(this OnErrorResumeNext @this, Action<String> appendLine)
            => String.Concat("On", " ", "Error", " ", "Resume", " ", "Next")
                     .Affect(s => { appendLine(s); });

        public static Unit ToVbScript(this OptionExplicitStatement @this, Action<String> appendLine)
            => String.Concat("Option", " ", "Explicit")
                     .Affect(s => { appendLine(s); });

        public static Unit ToVbScript(this RedimStatement @this, Action<String> appendLine)
            => String.Concat("Redim", " ", @this.Preserve ? "Preserve " : String.Empty, String.Join(", ", @this.Variables.Select(v => v.ToVbScript())))
                     .Affect(s => { appendLine(s); });

        public static Unit ToVbScript(this SelectCase @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine(@this switch
            {
                SelectCaseWhen caseWhen => String.Concat("Case", " ", String.Join(", ", caseWhen.When.Select(e => e.ToVbScript()))),
                SelectCaseElse caseElse => String.Concat("Case", " ", "Else"),
                _ => throw new Exception(UNEXPECTED_TYPE)
            });

            foreach (var stmt in @this)
                stmt.ToVbScript(indented);

            return Unit.Value;
        }

        public static Unit ToVbScript(this SelectStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine(String.Concat("Select", " ", "Case", " ", @this.Value.ToVbScript()));

            foreach (var @case in @this.Cases)
                @case.ToVbScript(indented);

            appendLine(String.Concat("End", " ", "Select"));

            return Unit.Value;
        }

        public static Unit ToVbScript(this VariableDeclaration @this, Action<String> appendLine)
            => String.Concat("Dim", " ", String.Join(", ", @this.Variables.Select(v => v.ToVbScript())))
                     .Affect(s => { appendLine(s); });

        public static Unit ToVbScript(this WhileStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine(String.Concat("While", " ", @this.Condition.ToVbScript()));

            foreach (var stmt in @this)
                stmt.ToVbScript(indented);

            appendLine("WEnd");

            return Unit.Value;
        }

        public static Unit ToVbScript(this WithStatement @this, Action<String> appendLine)
        {
            var indented = appendLine.Indented();

            appendLine(String.Concat("With", " ", @this.Expression.ToVbScript()));

            foreach (var stmt in @this)
                stmt.ToVbScript(indented);

            appendLine(String.Concat("End", " ", "With"));

            return Unit.Value;
        }

        public static Unit ToVbScript(this Statement @this, Action<String> appendLine)
           =>  @this is AssignStatement assign ? assign.ToVbScript(appendLine)
             : @this is CallStatement call ? call.ToVbScript(appendLine)
             : @this is ClassDeclaration @class ? @class.ToVbScript(appendLine)
             : @this is ConstantDeclaration constant ? constant.ToVbScript(appendLine)
             : @this is ConstantStatement constStmt ? constStmt.ToVbScript(appendLine)
             : @this is DoLoopStatement doloop ? doloop.ToVbScript(appendLine)
             : @this is DoLoopTestStatement dolooptest ? dolooptest.ToVbScript(appendLine)
             : @this is DoTestLoopStatement dotestloop ? dotestloop.ToVbScript(appendLine)
             : @this is EraseStatement erase ? erase.ToVbScript(appendLine)
             : @this is ExitStatement exit ? exit.ToVbScript(appendLine)
             : @this is FieldDeclaration field ? field.ToVbScript(appendLine)
             : @this is ForEachStatement @foreach ? @foreach.ToVbScript(appendLine)
             : @this is ForStatement @for ? @for.ToVbScript(appendLine)
             : @this is IfStatement @if ? @if.ToVbScript(appendLine)
             : @this is InlineCallStatement inline ? inline.ToVbScript(appendLine)
             : @this is MethodDeclaration method ? method.ToVbScript(appendLine)
             : @this is OnErrorGotoZero gotozero ? gotozero.ToVbScript(appendLine)
             : @this is OnErrorResumeNext resumenext ? resumenext.ToVbScript(appendLine)
             : @this is OptionExplicitStatement optionexplicit ? optionexplicit.ToVbScript(appendLine)
             : @this is RedimStatement redim ? redim.ToVbScript(appendLine)
             : @this is SelectStatement select ? select.ToVbScript(appendLine)
             : @this is VariableDeclaration variable ? variable.ToVbScript(appendLine)
             : @this is WhileStatement @while ? @while.ToVbScript(appendLine)
             : @this is WithStatement with  ? with.ToVbScript(appendLine)
             : throw new Exception(UNEXPECTED_TYPE);

        public static Unit ToVbScript(this Nysa.CodeAnalysis.VbScript.Semantics.Program @this, Action<String> appendLine)
        {
            foreach (var stmt in @this)
                stmt.ToVbScript(appendLine);

            return Unit.Value;
        }

    }

}