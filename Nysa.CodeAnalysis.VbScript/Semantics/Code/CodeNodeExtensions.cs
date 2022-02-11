using System;
using System.Linq;

using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class CodeNodeExtensions
    {
        private static IEnumerable<CodeNode> EmptySet = new CodeNode[] { };

        public static IEnumerable<CodeNode> Members(this AccessExpression @this)
            => @this switch
               {
                   NewObjectExpression  newObject   => newObject.Members(),
                   PathExpression       path        => path.Members(),
                   _ => throw new Exception("Unexpected type.")
               };

        public static IEnumerable<CodeNode> Members(this ArgumentDefinition @this)
            => Return.Enumerable<CodeNode>(@this.Name);
        public static IEnumerable<CodeNode> Members(this AssignStatement @this)
            => Return.Enumerable<CodeNode>(@this.Left, @this.Right);
        public static IEnumerable<CodeNode> Members(this CallStatement @this)
            => Return.Enumerable<CodeNode>(@this.AccessExpression);
        public static IEnumerable<CodeNode> Members(this ClassDeclaration @this)
            => Return.Enumerable<CodeNode>(@this.Name).Concat(@this.Statements);
        public static IEnumerable<CodeNode> Members(this Constant @this)
            => @this.Expression.Match(e => Return.Enumerable<CodeNode>(@this.Name, e),
                                      () => Return.Enumerable<CodeNode>(@this.Name));
        public static IEnumerable<CodeNode> Members(this ConstantDeclaration @this)
            => @this.Constants.Select(c => (CodeNode)c);
            
        public static IEnumerable<CodeNode> Members(this ConstantExpression @this)
            => @this switch
               {
                   ConstantOperation    operation   => operation.Members(),
                   LiteralValue         literal     => EmptySet,
                   _ => throw new Exception("Unexpected type.")
               };

        public static IEnumerable<CodeNode> Members(this ConstantOperation @this)
            => Return.Enumerable(@this.Operand);
        public static IEnumerable<CodeNode> Members(this ConstantStatement @this)
            => @this.Constants.Select(c => (CodeNode)c);
        public static IEnumerable<CodeNode> Members(this DoLoopStatement @this)
            => @this.Statements.Select(s => (CodeNode)s);
        public static IEnumerable<CodeNode> Members(this DoLoopTestStatement @this)
            => @this.Statements.Select(s => (CodeNode)s).Concat(Return.Enumerable<CodeNode>(@this.Condition));
        public static IEnumerable<CodeNode> Members(this DoTestLoopStatement @this)
            => Return.Enumerable<CodeNode>(@this.Condition).Concat(@this.Statements.Select(s => (CodeNode)s));
        public static IEnumerable<CodeNode> Members(this ElseBlock @this)
            => @this.Statements.Select(s => (CodeNode)s);
        public static IEnumerable<CodeNode> Members(this ElseIfBlock @this)
            => Return.Enumerable<CodeNode>(@this.Predicate).Concat(@this.Statements.Select(s => (CodeNode)s));
        public static IEnumerable<CodeNode> Members(this EraseStatement @this)
            => Return.Enumerable<CodeNode>(@this.Name);
        public static IEnumerable<CodeNode> Members(this ExitStatement @this)
            => EmptySet;

        public static IEnumerable<CodeNode> Members(this Expression @this)
            => @this switch
               {
                   AccessExpression     access      => access.Members(),
                   ConstantExpression   constant    => constant.Members(),
                   OperateExpression    operate     => operate.Members(),
                   PrecedenceExpression precedence  => precedence.Members(),
                   _ => throw new Exception("Unexpected type.")
               };

        public static IEnumerable<CodeNode> Members(this ExpressionList @this)
            => @this.Select(e => (CodeNode)e);
        public static IEnumerable<CodeNode> Members(this FieldDeclaration @this)
            => @this.Fields.Select(f => (CodeNode)f);
        public static IEnumerable<CodeNode> Members(this FinalElseBlock @this)
            => @this.Statements.Select(s => (CodeNode)s);
        public static IEnumerable<CodeNode> Members(this ForEachStatement @this)
            => Return.Enumerable<CodeNode>(@this.Variable, @this.In).Concat(@this.Statements.Select(s => (CodeNode)s));
        public static IEnumerable<CodeNode> Members(this ForStatement @this)
            => @this.Step.Match(s => Return.Enumerable<CodeNode>(@this.Variable, @this.From, @this.To, s),
                                () => Return.Enumerable<CodeNode>(@this.Variable, @this.From, @this.To))
                         .Concat(@this.Statements.Select(s => (CodeNode)s));

        // FunctionDeclaration is covered by MethodDeclaration

        public static IEnumerable<CodeNode> Members(this Identifier @this)
            => EmptySet;
        public static IEnumerable<CodeNode> Members(this IfStatement @this)
            => Return.Enumerable<CodeNode>(@this.Predicate, @this.Consequent)
                     .Concat(@this.Alternatives.Select(a => (CodeNode)a));
        public static IEnumerable<CodeNode> Members(this InlineCallStatement @this)
            => Return.Enumerable<CodeNode>(@this.AccessExpression);
        public static IEnumerable<CodeNode> Members(this LiteralValue @this)
            => EmptySet;

        public static IEnumerable<CodeNode> Members(this MethodDeclaration @this)
            => Return.Enumerable<CodeNode>(@this.Name)
                     .Concat(@this.Arguments.Select(a => (CodeNode)a))
                     .Concat(@this.Statements);

        public static IEnumerable<CodeNode> Members(this NewObjectExpression @this)
            => Return.Enumerable<CodeNode>(@this.Object);
        public static IEnumerable<CodeNode> Members(this OnErrorGotoZero @this)
            => EmptySet;
        public static IEnumerable<CodeNode> Members(this OnErrorResumeNext @this)
            => EmptySet;
        public static IEnumerable<CodeNode> Members(this OperateExpression @this)
            => @this.Operands.Select(o => (CodeNode)o);
        public static IEnumerable<CodeNode> Members(this OptionExplicitStatement @this)
            => EmptySet;

        public static IEnumerable<CodeNode> Members(this PathArguments @this)
            => @this.Select(a => (CodeNode)a);

        public static IEnumerable<CodeNode> Members(this PathExpression @this)
            => @this.Select(i => (CodeNode)i);

        public static IEnumerable<CodeNode> Members(this PathExpressionItem @this)
            => @this switch
               {
                   PathArguments    arguments   => arguments.Members(),
                   PathIdentifier   identifier  => identifier.Members(),
                   PathValue        value       => value.Members(),
                   PathWith         with        => with.Members(),
                   _ => throw new Exception("Unexpected type.")
               };

        public static IEnumerable<CodeNode> Members(this PathIdentifier @this)
            => EmptySet;
        public static IEnumerable<CodeNode> Members(this PathValue @this)
            => Return.Enumerable<CodeNode>(@this.Expression);
        public static IEnumerable<CodeNode> Members(this PathWith @this)
            => EmptySet;
        public static IEnumerable<CodeNode> Members(this PrecedenceExpression @this)
            => Return.Enumerable<CodeNode>(@this.Value);
        public static IEnumerable<CodeNode> Members(this Program @this)
            => @this.Statements.Select(s => (CodeNode)s);

        // PropertyDeclaration is covered by MethodDeclaration

        public static IEnumerable<CodeNode> Members(this RedimStatement @this)
            => @this.Variables.Select(v => (CodeNode)v);
        public static IEnumerable<CodeNode> Members(this RedimVariable @this)
            => Return.Enumerable<CodeNode>(@this.Name, @this.RankExpressions);

        public static IEnumerable<CodeNode> Members(this SelectCase @this)
            => @this switch
               {
                   SelectCaseElse caseElse => caseElse.Members(),
                   SelectCaseWhen caseWhen => caseWhen.Members(),
                   _ => throw new Exception("Unexpected type.")
               };

        public static IEnumerable<CodeNode> Members(this SelectCaseElse @this)
            => @this.Statements.Select(s => (CodeNode)s);
        public static IEnumerable<CodeNode> Members(this SelectCaseWhen @this)
            => Return.Enumerable<CodeNode>(@this.When).Concat(@this.Statements.Select(s => (CodeNode)s));
        public static IEnumerable<CodeNode> Members(this SelectStatement @this)
            => Return.Enumerable<CodeNode>(@this.Value).Concat(@this.Cases.Select(c => (CodeNode)c));

        public static IEnumerable<CodeNode> Members(this Statement @this)
            => @this switch
               {
                   AssignStatement          assign      => assign.Members(),
                   CallStatement            call        => call.Members(),
                   ClassDeclaration         classDec    => classDec.Members(),
                   ConstantDeclaration      constDec    => constDec.Members(),
                   ConstantStatement        constStm    => constStm.Members(),
                   DoLoopStatement          doLoop      => doLoop.Members(),
                   DoLoopTestStatement      doLoopTest  => doLoopTest.Members(),
                   DoTestLoopStatement      doTestLoop  => doTestLoop.Members(),
                   EraseStatement           erase       => erase.Members(),
                   ExitStatement            exit        => exit.Members(),
                   FieldDeclaration         field       => field.Members(),
                   ForEachStatement         forEach     => forEach.Members(),
                   ForStatement             forStm      => forStm.Members(),
                   IfStatement              ifStm       => ifStm.Members(),
                   InlineCallStatement      inline      => inline.Members(),
                   MethodDeclaration        method      => method.Members(),
                   OnErrorGotoZero          onErrZero   => onErrZero.Members(),
                   OnErrorResumeNext        onErrNext   => onErrNext.Members(),
                   OptionExplicitStatement  explicitStm => explicitStm.Members(),
                   RedimStatement           redim       => redim.Members(),
                   SelectStatement          select      => select.Members(),
                   VariableDeclaration      varDec      => varDec.Members(),
                   WhileStatement           whileStm    => whileStm.Members(),
                   WithStatement            with        => with.Members(),
                   _ => throw new Exception("Unexpected type.")
               };

        public static IEnumerable<CodeNode> Members(this StatementList @this)
            => @this.Select(s => (CodeNode)s);

        // SubroutineDeclaration is covered by MethodDeclaration

        public static IEnumerable<CodeNode> Members(this Variable @this)
            => @this.ArrayRanks.Match(r => Return.Enumerable<CodeNode>(@this.Name).Concat(r.Select(l => (CodeNode)l)),
                                      () => Return.Enumerable<CodeNode>(@this.Name));
        public static IEnumerable<CodeNode> Members(this VariableDeclaration @this)
            => @this.Variables.Select(v => (CodeNode)v);
        public static IEnumerable<CodeNode> Members(this WhileStatement @this)
            => Return.Enumerable(@this.Condition).Concat(@this.Statements.Select(s => (CodeNode)s));
        public static IEnumerable<CodeNode> Members(this WithStatement @this)
            => Return.Enumerable(@this.Expression).Concat(@this.Statements.Select(s => (CodeNode)s));

    }

}