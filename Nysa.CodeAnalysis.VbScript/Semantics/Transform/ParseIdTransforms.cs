using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Nysa.Logics;

using Nysa.Text.Lexing;
using Nysa.Text.Parsing;

using ParseId       = Nysa.Text.Identifier;
using Span          = Nysa.Text.TextSpan;
using SyntaxNode    = Nysa.Text.Parsing.Node;
using SyntaxToken   = Nysa.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class ParseIdTransforms
    {
        private static readonly String PROGRAM_ERROR = @"Program error.";

        private static TransformItem[] ToResults(this CodeNode @this)
            => new TransformItem[] { new SemanticItem(@this) };

        private static IEnumerable<T> CodeNodes<T>(this IEnumerable<TransformItem> @this)
            where T : CodeNode
        {
            foreach (var item in @this)
                if (item is SemanticItem s && s.Value is T node)
                    yield return node;
        }

        private static TransformItem[] ToResults<T>(this IEnumerable<T> @this)
            where T : CodeNode
            => @this.Select(cn => new SemanticItem(cn)).ToArray();

        private static LiteralValueTypes ToLiteralValueType(this TokenIdentifier identifier)
            =>   identifier.IsEqual(Id.Category.FloatLiteral) ? LiteralValueTypes.Float
               : identifier.IsEqual(Id.Category.StringLiteral) ? LiteralValueTypes.String
               : identifier.IsEqual(Id.Category.DateLiteral) ? LiteralValueTypes.Date
               : identifier.IsEqual(Id.Symbol.Nothing) ? LiteralValueTypes.Nothing
               : identifier.IsEqual(Id.Symbol.Null) ? LiteralValueTypes.Null
               : identifier.IsEqual(Id.Symbol.Empty) ? LiteralValueTypes.Empty
               : throw new ArgumentOutOfRangeException($"Invalid use of {nameof(ToLiteralValueType)}.");

        private static VisibilityTypes ToVisibilityType(this TokenIdentifier identifier)
            =>   identifier.IsEqual(Id.Symbol.Private) ? VisibilityTypes.Private
               : identifier.IsEqual(Id.Symbol.Public)  ? VisibilityTypes.Public
               :                                         throw new ArgumentOutOfRangeException($"Invalid use of {nameof(ToVisibilityType)}");

        private static Option<VisibilityTypes> ToVisbilityTypeOption(this SyntaxToken syntaxToken)
            =>   syntaxToken.Id.IsEqual(Id.Symbol.Private) ? VisibilityTypes.Private.Some()
               : syntaxToken.Id.IsEqual(Id.Symbol.Public)  ? VisibilityTypes.Public.Some()
               :                                             Option<VisibilityTypes>.None;

        private static Get<Option<(VisibilityTypes Visibility, Boolean IsDefault)>> BuildMethodAccess()
            => (b, i) =>
            {
                var next = (Index)(i.Value + 1);

                return (b[i] is TokenItem firstToken)
                       ? (firstToken.Value.Id.IsEqual(Id.Symbol.Public))
                         ? (b[next] is TokenItem secondToken && secondToken.Value.Id.IsEqual(Id.Symbol.Default))
                           ? ((VisibilityTypes.Public, true).Some(), next.Value + 1)
                           : ((VisibilityTypes.Public, false).Some(), i.Value + 1)
                         : (firstToken.Value.Id.IsEqual(Id.Symbol.Private))
                           ? ((VisibilityTypes.Private, false).Some(), i.Value + 1)
                           : (Option<(VisibilityTypes Visibility, Boolean IsDefault)>.None, i)
                       : (Option<(VisibilityTypes Visibility, Boolean IsDefault)>.None, i);
            };

        private static ConstantOperationTypes ToConstantOperationType(this TokenIdentifier identifier)
            =>   identifier.IsEqual(Id.Symbol.OpenParen) ? ConstantOperationTypes.precedence
               : identifier.IsEqual(Id.Symbol.Plus) ? ConstantOperationTypes.add
               : identifier.IsEqual(Id.Symbol.Minus) ? ConstantOperationTypes.subtract
               : throw new ArgumentOutOfRangeException($"Invalid use of {nameof(ToConstantOperationType)}");

        private static PropertyAccessTypes ToPropertyAccessType(this TokenIdentifier parseId)
            =>   parseId.IsEqual(Id.Symbol.Get) ? PropertyAccessTypes.Get
               : parseId.IsEqual(Id.Symbol.Let) ? PropertyAccessTypes.Let
               : parseId.IsEqual(Id.Symbol.Set) ? PropertyAccessTypes.Set
               : throw new Exception(PROGRAM_ERROR);

        private static ExitTypes ToExitType(this TokenIdentifier parseId)
            =>   parseId.IsEqual(Id.Symbol.Do) ? ExitTypes.Do
               : parseId.IsEqual(Id.Symbol.For) ? ExitTypes.For
               : parseId.IsEqual(Id.Symbol.Function) ? ExitTypes.Function
               : parseId.IsEqual(Id.Symbol.Property) ? ExitTypes.Property
               : parseId.IsEqual(Id.Symbol.Sub) ? ExitTypes.Sub
               : throw new Exception(PROGRAM_ERROR);


        private static TransformItem[] CreateArguments(TransformContext context, TransformItem[] members)
        {
            var current = Index.Start;
            var mod     = Option<ArgumentModifiers>.None;
            var id      = (Identifier?)null;
            var sfx     = false;
            var items   = new List<TransformItem>();

            while (current.Value < members.Length)
            {
                if (members[current] is TokenItem token)
                {
                    if (token.Value.Id.IsEqual(Id.Symbol.Comma))
                    {
                        if (id == null)
                            throw new Exception(With.MISSING_NODE);

                        items.Add((SemanticItem)new ArgumentDefinition(id.Node, mod, id, sfx));

                        mod = Option<ArgumentModifiers>.None;
                        id = null;
                        sfx = false;
                    }
                    else if (token.Value.Id.IsEqual(Id.Symbol.ByVal))
                        mod = ArgumentModifiers.ByVal.Some();
                    else if (token.Value.Id.IsEqual(Id.Symbol.ByRef))
                        mod = ArgumentModifiers.ByRef.Some();
                    else if (id != null && token.Value.Id.IsEqual(Id.Symbol.OpenParen))
                        sfx = true;
                }
                else if (members[current] is SemanticItem node && node.Value is Identifier nextId)
                {
                    id = nextId;
                }

                current = current.Value + 1;
            }

            if (id != null)
                items.Add((SemanticItem)new ArgumentDefinition(id.Node, mod, id, sfx));

            return items.ToArray();
        }

        private static (IEnumerable<Constant> Items, Index Remainder) BuildConstants(TransformItem[] members, Index start)
        {
            var current = start;
            var id = (Identifier?)null;
            var def = Option<ConstantExpression>.None;
            var items = new List<Constant>();

            while (current.Value < members.Length)
            {
                if (members[current] is TokenItem token)
                {
                    if (token.Value.Id.IsEqual(Id.Symbol.Comma))
                    {
                        if (id == null)
                            throw new Exception(With.MISSING_NODE);

                        items.Add(new Constant(id.Node, id, def));

                        id = null;
                        def = Option<ConstantExpression>.None;
                    }
                }
                else if (members[current] is SemanticItem node)
                {
                    if (node.Value is Identifier nodeId)
                        id = nodeId;
                    else if (node.Value is ConstantExpression constExpr)
                        def = constExpr.Some();
                }

                current = current.Value + 1;
            }

            if (id != null)
                items.Add(new Constant(id.Node, id, def));

            return (items, current);
        }

        // private static (IEnumerable<SelectCase> Items, Index Remainder) BuildCases(TransformItem[] members, Index start)
        // {
        //     var current = start;
            
        // }


        private enum PathTransitionActions : Int32
        {
            None,
            SaveId,
            SaveArgs,
            SaveWith,
            SaveValue
        }

        private class PathTransition
        {
            public PathTransitionActions    Action      { get; set; }
            public ParseId                  NewState    { get; set; }

            public PathTransition(PathTransitionActions action, ParseId newState)
            {
                this.Action = action;
                this.NewState = newState;
            }
        }

        private static PathTransition ThenState(this PathTransitionActions @this, ParseId newState)
            => new PathTransition(@this, newState);

        private static Dictionary<ParseId, PathTransition> ToStateDo(params (ParseId ToState, PathTransition Transition)[] values)
            => values.ToDictionary(k => k.ToState, v => v.Transition);
        private static Dictionary<ParseId, Dictionary<ParseId, PathTransition>> WhenStates(params (ParseId FromState, Dictionary<ParseId, PathTransition> ActionStates)[] stateTrans)
            => stateTrans.ToDictionary(k => k.FromState, v => v.ActionStates);

        private static readonly Dictionary<ParseId, Dictionary<ParseId, PathTransition>> LeftExprStates
            = WhenStates((Id.Rule.LeftExpr,
                          ToStateDo((Id.Symbol.Dot, PathTransitionActions.SaveWith.ThenState(Id.Symbol.Dot)),
                                    (Id.Category.ID, PathTransitionActions.SaveId.ThenState(Id.Category.ID)))),
                         (Id.Symbol.Dot,
                          ToStateDo((Id.Category.ID, PathTransitionActions.SaveId.ThenState(Id.Category.ID)))),
                         (Id.Category.ID,
                          ToStateDo((Id.Symbol.Dot, PathTransitionActions.None.ThenState(Id.Symbol.Dot)),
                                    (Id.Symbol.OpenParen, PathTransitionActions.None.ThenState(Id.Symbol.OpenParen)))),
                         (Id.Symbol.OpenParen,
                          ToStateDo((Id.Symbol.Comma, PathTransitionActions.None.ThenState(Id.Symbol.OpenParen)),
                                    (Id.Symbol.CloseParen, PathTransitionActions.SaveArgs.ThenState(Id.Category.ID))))
                        );

        private static readonly Dictionary<ParseId, Dictionary<ParseId, PathTransition>> ValueExprStates
            = WhenStates((Id.Rule.Value,
                          ToStateDo((Id.Symbol.OpenParen, PathTransitionActions.None.ThenState(Id.Rule.Value)),
                                    (Id.Symbol.CloseParenDot, PathTransitionActions.SaveValue.ThenState(Id.Symbol.Dot)))),
                         (Id.Symbol.Dot,
                          ToStateDo((Id.Category.ID, PathTransitionActions.SaveId.ThenState(Id.Category.ID)))),
                         (Id.Category.ID,
                          ToStateDo((Id.Symbol.Dot, PathTransitionActions.None.ThenState(Id.Symbol.Dot)),
                                    (Id.Symbol.OpenParen, PathTransitionActions.None.ThenState(Id.Symbol.OpenParen)))),
                         (Id.Symbol.OpenParen,
                          ToStateDo((Id.Symbol.Comma, PathTransitionActions.None.ThenState(Id.Symbol.OpenParen)),
                                    (Id.Symbol.CloseParen, PathTransitionActions.SaveArgs.ThenState(Id.Category.ID))))
                        );

        private static readonly Dictionary<ParseId, Dictionary<ParseId, PathTransition>> SubCallStates
            = WhenStates((Id.Rule.SubCallStmt,
                          ToStateDo((Id.Symbol.Dot, new PathTransition(PathTransitionActions.SaveWith, Id.Symbol.Dot)),
                                    (Id.Category.ID, new PathTransition(PathTransitionActions.SaveId, Id.Category.ID)))),
                         (Id.Symbol.Dot,
                          ToStateDo((Id.Category.ID, PathTransitionActions.SaveId.ThenState(Id.Category.ID)))),
                         (Id.Category.ID,
                          ToStateDo((Id.Symbol.Dot, PathTransitionActions.None.ThenState(Id.Symbol.Dot)),
                                    (Id.Symbol.OpenParen, PathTransitionActions.None.ThenState(Id.Symbol.OpenParen)))),
                         (Id.Symbol.OpenParen,
                          ToStateDo((Id.Symbol.Comma, PathTransitionActions.None.ThenState(Id.Symbol.OpenParen)),
                                    (Id.Symbol.CloseParen, PathTransitionActions.SaveArgs.ThenState(Id.Category.ID)))),
                         (Id.Rule.CommaExprList,
                          ToStateDo((Id.Symbol.Comma, PathTransitionActions.None.ThenState(Id.Rule.CommaExprList))))
                        );

        private static Get<IEnumerable<PathExpressionItem>> CreatePathExprProcessor(ParseId startState, Dictionary<ParseId, Dictionary<ParseId, PathTransition>> stateTrans)
            => (i, r) =>
            {
                var current     = r;
                var items       = new List<PathExpressionItem>();
                var state       = startState;
                var args        = new List<Expression>();
                var celToken    = new SyntaxToken();

                while (current.Value < i.Length)
                {
                    var item = i[current];

                    if (item is TokenItem token)
                    {
                        var tid = token.Value.Id.Values().FirstOrNone(i => stateTrans.ContainsKey(state) && stateTrans[state].ContainsKey(i));

                        if (tid is Some<ParseId> someId && stateTrans.ContainsKey(state) && stateTrans[state].ContainsKey(someId.Value))
                        {
                            var trans = stateTrans[state][someId.Value];

                            switch (trans.Action)
                            {
                                case PathTransitionActions.SaveId:
                                    items.Add(new PathIdentifier(token.Value, token.Value.Span.ToString()));
                                    break;
                                case PathTransitionActions.SaveWith:
                                    items.Add(new PathWith(token.Value));
                                    break;
                                case PathTransitionActions.SaveArgs:
                                    items.Add(new PathArguments(token.Value, true, args));
                                    args = new List<Expression>();
                                    break;
                                case PathTransitionActions.SaveValue:
                                    items.Add(new PathValue(token.Value, args[0]));
                                    args = new List<Expression>();
                                    break;
                                default:
                                    break;
                            }

                            state = trans.NewState;

                            if (state == Id.Rule.CommaExprList)
                                celToken = token.Value; // save first id token as source for PathArguments (below)
                        }
                        else
                        {
                            throw new Exception(PROGRAM_ERROR);
                        }
                    }
                    else if (item is SemanticItem checkStart && checkStart.Value is SubroutineArgumentStart start)
                    {
                        if (start.Argument is Some<Expression> some)
                            args.Add(some.Value);

                        state = Id.Rule.CommaExprList;
                    }
                    else if (item is SemanticItem node)
                    {
                        if (node.Value is Expression expr)
                            args.Add(expr);
                        else
                            throw new Exception(PROGRAM_ERROR);
                    }
                    else
                    { throw new Exception(PROGRAM_ERROR); }

                    current = current.Value + 1;
                }

                if (state == Id.Rule.CommaExprList)
                    items.Add(new PathArguments(celToken, false, args));

                return (items, current);
            };

        private static readonly Get<IEnumerable<PathExpressionItem>> GetValueExprItems = CreatePathExprProcessor(Id.Rule.Value, ValueExprStates);
        private static readonly Get<IEnumerable<PathExpressionItem>> GetLeftExprItems = CreatePathExprProcessor(Id.Rule.LeftExpr, LeftExprStates);
        private static readonly Get<IEnumerable<PathExpressionItem>> GetSubCallExprItems = CreatePathExprProcessor(Id.Rule.SubCallStmt, SubCallStates);

        private static readonly Transform SubCallStmtNormalize
            = (c, m) =>
            {
                if (!m.Any(i => i is SemanticItem node && node.Value is SubroutineArgumentStart))
                {
                    TransformItem[] rebuild(IEnumerable<TransformItem> pre, SyntaxToken source, Option<Expression> firstParam, IEnumerable<TransformItem> post)
                    {
                        return pre.Concat(Return.Enumerable((SemanticItem)new SubroutineArgumentStart(source, firstParam.Map(ex => (Expression)new PrecedenceExpression(source, ex)))))
                                  .Concat(post)
                                  .ToArray();
                    }

                    m = With.Parts(Until.Token(Id.Symbol.OpenParen),
                                   Expect.TokenOf(Id.Symbol.OpenParen),
                                   Maybe.Node<Expression>(),
                                   Expect.TokenOf(Id.Symbol.CloseParen),
                                   Until.NoMore())
                            .Make((ns, pre, op, mex, cp, post) => rebuild(pre, op, mex, post))(c, m);
                }

                return m;
            };

        private static readonly Transform InlineStatementsTransform
            = (c, m) => c.Previous
                         .Match(p => p.Node.Id == Id.Rule.InlineStmtList
                                     ? m
                                     : new TransformItem[] { (SemanticItem)new StatementList(c.Node, m.CodeNodes<Statement>()) },
                                () => throw new Exception(PROGRAM_ERROR));

        private static readonly Transform ExpressionsTransform
            = (c, m) => c.Previous
                         .Match(p => p.Node.Id == Id.Rule.ExprList
                                     ? m
                                     : new TransformItem[] { (SemanticItem)new ExpressionList(c.Node, m.CodeNodes<Expression>()) },
                                () => throw new Exception(PROGRAM_ERROR));

        private static readonly Transform ElseBlocksTransform
            = (TokenCheck:  Maybe.TokenOf(Id.Symbol.ElseIf),
               ElseIfBuild: With.Parts(Skip.ToExpected<Expression>(),
                                       Skip.ToZeroOrMore<Statement>(s => !(s is ElseBlock)),
                                       Skip.ToZeroOrMore<ElseBlock>()),
               ElseBuild:   With.Parts(Skip.ToZeroOrMore<Statement>())
              ).Make(t => new Transform((x, m) => m.Length == 0
                                                  ? m
                                                  : t.TokenCheck(m, Index.Start)
                                                     .Item
                                                     .Match(ei => t.ElseIfBuild.Make((ns, a, b, c) => (new ElseIfBlock(ns, a, b)).Enumerable<ElseBlock>().Concat(c).ToResults())(x, m),
                                                            () => t.ElseBuild.Make((ns, a) => new TransformItem[] { (SemanticItem)new FinalElseBlock(ns, a) })(x, m))) );

        private static readonly Transform EraseStmtTransform
            = With.Parts(Skip.ToExpected<Identifier>()).Make((ns, a) => new EraseStatement(ns, a));

        private static readonly Transform InlineStmtTransform
            = (c, m) => m.Length == 1 ? m : EraseStmtTransform(c, m);

        private static readonly Transform FinalElseTransform
            = With.Parts(Skip.ToExpected<StatementList>()).Make((ns, a) => new FinalElseBlock(ns, a));

        private static readonly Transform ElseOptTransform
            = (c, m) => m.Length == 0 ? m : FinalElseTransform(c, m);

        // This logic works for the binary operations of the VBScript grammar because they create left recursive nodes.
        private static Transform SingleOpTransform(this OperationTypes operationType)
            => (c, m) => m.Length > 0 && m[0] is SemanticItem node && node.Value is OperateExpression op && op.Operation == operationType
                         ? (new OperateExpression(c.Node, operationType, op.Operands.Concat(m.Skip(1).CodeNodes<Expression>()))).ToResults()
                         : (new OperateExpression(c.Node, operationType, m.CodeNodes<Expression>())).ToResults();

        private static readonly Transform SignOpTransform
            = With.Condition((c, i) => i[Index.Start] is SemanticItem,
                             With.Parts().Return(),
                             With.Parts(Expect.TokenOf(Id.Symbol.Plus, Id.Symbol.Minus),
                                        Skip.ToZeroOrMore<Expression>())
                                 .Make((ns, t, e) => new OperateExpression(ns,
                                                                           t.Id.IsEqual(Id.Symbol.Plus) ? OperationTypes.SignPositive : OperationTypes.SignNegative,
                                                                           e)));

        private static readonly Transform AddOpTransform
            = (c, m) => m.Length > 2
                        ? m[1].Match(cn => m,
                                     st =>   st.Id.IsEqual(Id.Symbol.Plus)  ? SingleOpTransform(OperationTypes.Add)(c, m)
                                           : st.Id.IsEqual(Id.Symbol.Minus) ? SingleOpTransform(OperationTypes.Subtract)(c, m)
                                           : m)
                        : m;

        private static readonly Transform MultOpTransform
            = (c, m) => m.Length > 2
                        ? m[1].Match(cn => m,
                                     st =>   st.Id.IsEqual(Id.Symbol.Mult) ? SingleOpTransform(OperationTypes.Multiply)(c, m)
                                           : st.Id.IsEqual(Id.Symbol.Div)  ? SingleOpTransform(OperationTypes.Divide)(c, m)
                                           :                                 m)
                        : m;

        private static Option<OperationTypes> ToOperationType(TokenIdentifier id, TransformItem next)
            =>   id.IsEqual(Id.Symbol.GTE) ||
                 id.IsEqual(Id.Symbol.EGT)    ? OperationTypes.GreaterOrEqual.Some()
               : id.IsEqual(Id.Symbol.EGT)    ? OperationTypes.GreaterOrEqual.Some()
               : id.IsEqual(Id.Symbol.LTE) ||
                 id.IsEqual(Id.Symbol.ELT)    ? OperationTypes.LesserOrEqual.Some()
               : id.IsEqual(Id.Symbol.GT)     ? OperationTypes.Greater.Some()
               : id.IsEqual(Id.Symbol.LT)     ? OperationTypes.Lesser.Some()
               : id.IsEqual(Id.Symbol.NEQ)    ? OperationTypes.NotEqual.Some()
               : id.IsEqual(Id.Symbol.Equals) ? OperationTypes.Equal.Some()
               : id.IsEqual(Id.Symbol.Is)     ? next.Match(cn => OperationTypes.Is, st => OperationTypes.IsNot).Some()
               :                                Option<OperationTypes>.None;

        private static readonly Transform CompareOpTransform
            = (c, i) => i.Length > 2
                        ? i[1].Match(cn => i,
                                     st => ToOperationType(st.Id, i[2]).Match(op => (new OperateExpression(c.Node, op, i.CodeNodes<Expression>())).ToResults(),
                                                                              () => i))
                        : i;

        private static readonly Transform SelectCaseTransform
            = With.Optional(With.Condition((c, i) => i.Length >= 2 && i[1] is TokenItem,
                                           With.Parts(Expect.TokenOf(Id.Symbol.Case),
                                                      Expect.TokenOf(Id.Symbol.Else),
                                                      Skip.ToZeroOrMore<Statement>())
                                               .Make((b, ta, tb, s) => new SelectCaseElse(b, s)),
                                           With.Parts(Expect.TokenOf(Id.Symbol.Case),
                                                      Skip.ToExpected<ExpressionList>(),
                                                      Skip.ToZeroOrMore<Statement>(s => !(s is SelectCase)),
                                                      Skip.ToZeroOrMore<SelectCase>())
                                               .Make((b, t, el, s, c) => Return.Enumerable<SelectCase>(new SelectCaseWhen(b, el, s)).Concat(c))));

        private static readonly Transform VariableTransform
            = With.Parts(Skip.ToExpected<Identifier>(),
                         Maybe.TokenOf(Id.Symbol.OpenParen),
                         Skip.ToZeroOrMore<LiteralValue>())
                  .Make((ns, a, b, c) => new Variable(ns, a, b.Map(t => c)));

        private static readonly Transform WhileTransform
            = With.Parts(Expect.TokenOf(Id.Symbol.While),
                         Expect.Node<Expression>(),
                         Skip.ToZeroOrMore<Statement>())
                  .Make((ns, tw, e, s) => new WhileStatement(ns, e, s));

        private static LoopTypes ToLoopType(this TokenIdentifier id)
            => id.IsEqual(Id.Symbol.While) ? LoopTypes.While : LoopTypes.Until;

        private static readonly Transform DoLoopTransform
            = With.Parts(Expect.TokenOf(Id.Symbol.Do),
                         Maybe.TokenOf(Id.Symbol.While, Id.Symbol.Until),
                         Maybe.Node<Expression>(),
                         Take<Statement>.ZeroOrMoreUntil(Id.Symbol.Loop),
                         Expect.TokenOf(Id.Symbol.Loop),
                         Maybe.TokenOf(Id.Symbol.While, Id.Symbol.Until),
                         Maybe.Node<Expression>())
                  .Make((ns, _, t1, e1, s, l, t2, e2) => t1.Match(mt1 => (CodeNode)new DoTestLoopStatement(ns, ToLoopType(mt1.Id), e1.Match(e => e, () => throw new Exception(With.MISSING_NODE)), s),
                                                                  ()  => t2.Match(mt2 => (CodeNode)new DoLoopTestStatement(ns, s, mt2.Id.ToLoopType(), e2.Match(e => e, () => throw new Exception(With.MISSING_NODE))),
                                                                                  ()  => (CodeNode)new DoLoopStatement(ns, s))));

        private static readonly Transform ForTransform
            = With.Parts(Skip.ToExpected<Identifier>(),
                         Skip.ToExpected<Expression>(),
                         Skip.ToExpected<Expression>(),
                         Maybe.TokenOf(Id.Symbol.Step),
                         Maybe.Node<Expression>(),
                         Skip.ToZeroOrMore<Statement>())
                  .Make((ns, i, f, t, tk, p, s) => new ForStatement(ns, i, f, t, p, s));

        private static readonly Transform ForEachTransform
            = With.Parts(Skip.ToExpected<Identifier>(),
                         Skip.ToExpected<Expression>(),
                         Skip.ToZeroOrMore<Statement>())
                  .Make((ns, i, n, s) => new ForEachStatement(ns, i, n, s));

        private static SyntaxToken StartDot(this SyntaxToken startsWithDot)
            => startsWithDot.Span.Make(o => new SyntaxToken(new Span(o.Source, o.Position, 1), Id.Symbol.Dot));
        private static SyntaxToken EndDot(this SyntaxToken endsWithDot)
            => endsWithDot.Span.Make(o => new SyntaxToken(new Span(o.Source, o.Position + (o.Length - 1), 1), Id.Symbol.Dot));
        private static SyntaxToken WithoutStartDot(this SyntaxToken startsWithDot, ParseId newId)
            => startsWithDot.Span.Make(o => new SyntaxToken(new Span(o.Source, o.Position + 1, o.Length - 1), newId));
        private static SyntaxToken WithoutEndDot(this SyntaxToken endsWithDot, ParseId newId)
            => endsWithDot.Span.Make(o => new SyntaxToken(new Span(o.Source, o.Position, o.Length - 1), newId));

        private static PathExpression CheckInlineCallArgs(this PathExpression @this, SyntaxNode basis)
        {
            // if a precedence expression is the only argument in an inline call, then it's value can be lifted into the arguments collection
            if (@this.Count == 2 && @this[1] is PathArguments pargs && pargs.Count == 1 && pargs[0] is PrecedenceExpression precExpr)
                return new PathExpression(basis, Return.Enumerable(@this[0], new PathArguments(((Some<SyntaxToken>)precExpr.Token).Value, true, Return.Enumerable(precExpr.Value))));
            else
                return @this;
        }

        private static Get<IEnumerable<SyntaxToken>> DotSplit(this Get<SyntaxToken> dottenToken)
            => (i, r) =>
            {
                var v = dottenToken(i, r);

                return   (v.Item.Id.IsEqual(Id.Category.ID))          ? (new SyntaxToken[] { v.Item }, v.Remainder)
                       : (v.Item.Id.IsEqual(Id.Category.IDDot))       ? (new SyntaxToken[] { v.Item.WithoutEndDot(Id.Category.ID), v.Item.EndDot() }, v.Remainder)
                       : (v.Item.Id.IsEqual(Id.Category.DotID))       ? (new SyntaxToken[] { v.Item.StartDot(), v.Item.WithoutStartDot(Id.Category.ID) }, v.Remainder)
                       : (v.Item.Id.IsEqual(Id.Category.DotIDDot))    ? (new SyntaxToken[] { v.Item.StartDot(), v.Item.WithoutStartDot(Id.Category.IDDot).WithoutEndDot(Id.Category.ID), v.Item.EndDot() }, v.Remainder)
                       : (v.Item.Id.IsEqual(Id.Symbol.CloseParenDot)) ? (new SyntaxToken[] { v.Item.WithoutEndDot(Id.Symbol.CloseParen), v.Item.EndDot() }, v.Remainder)
                       :                                                (new SyntaxToken[] { v.Item }, v.Remainder);
            };

        private static Get<TokenItem> ChangeToId(this Get<SyntaxToken> keyword)
            => (i, r) =>
            {
                var v = keyword(i, r);

                return v.Item.Id.IsEqual(Id.Category.ID)
                       ? v
                       : v.Item.Span.Make(o => ((TokenItem)new SyntaxToken(o, Id.Category.ID.ToTokenIdentifier()), v.Remainder));
            };

        private static readonly Dictionary<ParseId, Transform> NodeBuilders = new Dictionary<ParseId, Transform>()
        {
            { Id.Rule.SafeKeywordID,     With.Parts(Expect.Token().ChangeToId()).Make((ns, i) => new TransformItem[] { i }) },
            { Id.Rule.KeywordID,         With.Parts(Expect.Token().ChangeToId()).Make((ns, i) => new TransformItem[] { i }) },
            { Id.Rule.OptionExplicit,    With.Parts().Make(ns => new OptionExplicitStatement(ns)) },
            { Id.Rule.BoolLiteral,       With.Parts(Expect.TokenValue()).Make((ns, tv) => new LiteralValue(ns, LiteralValueTypes.Boolean, tv)) },
            { Id.Rule.Nothing,           With.Parts(Expect.Token()).Make((ns, t) => new LiteralValue(ns, t.Id.ToLiteralValueType(), t.Span.ToString())) },
            { Id.Rule.IntLiteral,        With.Parts(Expect.TokenValue()).Make((ns, tv) => new LiteralValue(ns, LiteralValueTypes.Integer, tv)) },
            { Id.Rule.ConstExpr,         With.Either(cn => cn,
                                                     (sb, st) => new LiteralValue(sb, st.Id.ToLiteralValueType(), st.Span.ToString()))
                                             .ToTransform() },
            { Id.Rule.ExtendedID,        With.Parts(Expect.TokenValue()).Make((ns, tv) => new Identifier(ns, tv)) },
            { Id.Rule.QualifiedID,       With.Concat(Expect.Token().DotSplit(),
                                                     Expect.Tokens(),
                                                     t => (TokenItem)t) },
            { Id.Rule.QualifiedIDTail,   With.Condition((c, i) => i.Length == 1,
                                                        With.Parts().Return(),
                                                        With.Concat(Expect.TokenOf(Id.Category.IDDot).DotSplit(),
                                                                    Expect.Tokens(),
                                                                    t => (TokenItem)t)) },
            { Id.Rule.FieldID,           With.Parts(Expect.TokenValue()).Make((ns, tv) => new Identifier(ns, tv)) },
            { Id.Rule.Value,             With.Condition((c, i) => i.Length == 1,
                                                        With.Parts().Return(),
                                                        With.Condition((c, i) => i.Length == 3,
                                                                       With.Parts(Expect.TokenOf(Id.Symbol.OpenParen),
                                                                                  Expect.Node<Expression>(),
                                                                                  Expect.TokenOf(Id.Symbol.CloseParen))
                                                                           .Make((ns, o, e, c) => new PrecedenceExpression(o, e)),
                                                                       With.Parts(GetValueExprItems)
                                                                           .Make((ns, i) => new PathExpression(ns, i)))) },
            { Id.Rule.FieldName,         VariableTransform },
            { Id.Rule.VarName,           VariableTransform },
            { Id.Rule.ClassDecl,         With.Parts(Skip.ToExpected<Identifier>(), Skip.ToZeroOrMore<Statement>())
                                             .Make((ns, a, b) => new ClassDeclaration(ns, a, b)) },
            { Id.Rule.FieldDecl,         With.Parts(Expect.TokenOf(Id.Symbol.Private, Id.Symbol.Public), Skip.ToZeroOrMore<Variable>())
                                             .Make((ns, a, b) => new FieldDeclaration(ns, a.Id.ToVisibilityType(), b)) },
            { Id.Rule.ConstExprDef,      With.Either()
                                             .Make(nr => nr, (ns, st, br) => (new ConstantOperation(ns, st.Id.ToConstantOperationType(), ((SemanticItem)br).Value as ConstantExpression))) },
            { Id.Rule.VarDecl,           With.Parts(Skip.ToZeroOrMore<Variable>())
                                             .Make((ns, a) => (new VariableDeclaration(ns, a))) },
            { Id.Rule.ConstDecl,         With.Parts(Maybe.TokenOf(Id.Symbol.Public, Id.Symbol.Private),
                                                    BuildConstants)
                                             .Make((nr, a, b) => new ConstantDeclaration(nr, a.Bind(ToVisbilityTypeOption), b)) },
            { Id.Rule.BlockConstDecl,    With.Parts(Expect.TokenOf(Id.Symbol.Const),
                                                    BuildConstants)
                                             .Make((ns, a, b) => new ConstantStatement(ns, b)) },
            { Id.Rule.MethodArgList,     CreateArguments },
            { Id.Rule.PropertyDecl,      With.Parts(BuildMethodAccess(),
                                                    Expect.TokenOf(Id.Symbol.Property),
                                                    Expect.TokenOf(Id.Symbol.Get, Id.Symbol.Let, Id.Symbol.Set),
                                                    Skip.ToExpected<Identifier>(),
                                                    Skip.ToZeroOrMore<ArgumentDefinition>(),
                                                    Skip.ToZeroOrMore<Statement>())
                                             .Make((ns, a, b, c, d, e, f) => new PropertyDeclaration(ns, a.Map(p => p.Visibility), a.Match(p => p.IsDefault, () => false), c.Id.ToPropertyAccessType(), d, e, f)) },
            { Id.Rule.SubDecl,           With.Parts(BuildMethodAccess(),
                                                    Skip.ToExpected<Identifier>(),
                                                    Skip.ToZeroOrMore<ArgumentDefinition>(),
                                                    Skip.ToZeroOrMore<Statement>())
                                             .Make((ns, a, b, c, d) => new SubroutineDeclaration(ns, a.Map(p => p.Visibility), a.Match(p => p.IsDefault, () => false), b, c, d)) },
            { Id.Rule.FunctionDecl,      With.Parts(BuildMethodAccess(),
                                                    Skip.ToExpected<Identifier>(),
                                                    Skip.ToZeroOrMore<ArgumentDefinition>(),
                                                    Skip.ToZeroOrMore<Statement>())
                                             .Make((ns, a, b, c, d) => new FunctionDeclaration(ns, a.Map(p => p.Visibility), a.Match(p => p.IsDefault, () => false), b, c, d)) },
            { Id.Rule.SubSafeAddExpr,    AddOpTransform },
            { Id.Rule.SubSafeAndExpr,    OperationTypes.And.SingleOpTransform() },
            { Id.Rule.SubSafeCompareExpr, CompareOpTransform },
            { Id.Rule.SubSafeConcatExpr, OperationTypes.Concatenate.SingleOpTransform() },
            { Id.Rule.SubSafeImpExpr,    OperationTypes.Implication.SingleOpTransform() },
            { Id.Rule.SubSafeEqvExpr,    OperationTypes.Equivalence.SingleOpTransform() },
            { Id.Rule.SubSafeExpExpr,    OperationTypes.Exponentiate.SingleOpTransform() },
            { Id.Rule.SubSafeIntDivExpr, OperationTypes.IntDivide.SingleOpTransform() },
            { Id.Rule.SubSafeModExpr,    OperationTypes.Mod.SingleOpTransform() },
            { Id.Rule.SubSafeMultExpr,   MultOpTransform },
            { Id.Rule.SubSafeNotExpr,    OperationTypes.Not.SingleOpTransform() },
            { Id.Rule.SubSafeXorExpr,    OperationTypes.ExclusiveOr.SingleOpTransform() },
            { Id.Rule.SubSafeOrExpr,     OperationTypes.Or.SingleOpTransform() },
            { Id.Rule.SubSafeUnaryExpr,  SignOpTransform },
            { Id.Rule.SubSafeExprOpt,    With.Parts(Maybe.Node<Expression>())
                                             .Make((ns, e) => new SubroutineArgumentStart(ns, e)) },
            { Id.Rule.AddExpr,           AddOpTransform },
            { Id.Rule.AndExpr,           OperationTypes.And.SingleOpTransform() },
            { Id.Rule.CompareExpr,       CompareOpTransform },
            { Id.Rule.ConcatExpr,        OperationTypes.Concatenate.SingleOpTransform() },
            { Id.Rule.EqvExpr,           OperationTypes.Equivalence.SingleOpTransform() },
            { Id.Rule.ExpExpr,           OperationTypes.Exponentiate.SingleOpTransform() },
            { Id.Rule.ImpExpr,           OperationTypes.Implication.SingleOpTransform() },
            { Id.Rule.IntDivExpr,        OperationTypes.IntDivide.SingleOpTransform() },
            { Id.Rule.ModExpr,           OperationTypes.Mod.SingleOpTransform() },
            { Id.Rule.MultExpr,          MultOpTransform },
            { Id.Rule.NotExpr,           OperationTypes.Not.SingleOpTransform() },
            { Id.Rule.OrExpr,            OperationTypes.Or.SingleOpTransform() },
            { Id.Rule.XorExpr,           OperationTypes.ExclusiveOr.SingleOpTransform() },
            { Id.Rule.UnaryExpr,         SignOpTransform },
            { Id.Rule.IndexOrParamsDot,  With.Concat(Until.Token(Id.Symbol.CloseParenDot),
                                                     Expect.Token().DotSplit().Cast(st => (TransformItem)(TokenItem)st)) },
            { Id.Rule.LeftExpr,          With.Condition((c, i) => i.Length == 1 && i[0] is SemanticItem item && item.Value is AccessExpression,
                                                        With.Parts().Return(),
                                                        With.Parts(GetLeftExprItems)
                                                            .Make((ns, i) => new PathExpression(ns, i))) },
            { Id.Rule.ExprList,          ExpressionsTransform },
            { Id.Rule.NewObjectExpr,     With.Parts(Expect.TokenOf(Id.Symbol.New),
                                                    Skip.ToExpected<AccessExpression>())
                                             .Make((ns, a, b) => new NewObjectExpression(ns, b)) },
            { Id.Rule.ExitStmt,          With.Parts(Expect.TokenOf(Id.Symbol.Exit),
                                                    Expect.Token())
                                             .Make((ns, a, b) => new ExitStatement(ns, b.Id.ToExitType())) },
            { Id.Rule.ErrorStmt,         With.Parts(Expect.TokenOf(Id.Symbol.On),
                                                    Expect.TokenOf(Id.Symbol.Error),
                                                    Expect.Token())
                                             .Make((ns, a, b, c) => c.Id.IsEqual(Id.Symbol.Resume)
                                                                    ? (CodeNode)new OnErrorResumeNext(ns)
                                                                    : (CodeNode)new OnErrorGotoZero(ns)) },
            { Id.Rule.InlineStmt,        InlineStmtTransform },
            { Id.Rule.InlineStmtList,    InlineStatementsTransform },
            { Id.Rule.ElseOpt,           ElseOptTransform },
            { Id.Rule.ElseStmtList,      ElseBlocksTransform },
            { Id.Rule.InlineIfStmt,      With.Parts(Skip.ToExpected<Expression>(),
                                                    Skip.ToExpected<StatementList>(),
                                                    Maybe.Node<FinalElseBlock>())
                                             .Make((ns, a, b, c) => new IfStatement(ns, a, b, Enumerable.Empty<ElseIfBlock>(), c)) },
            { Id.Rule.IfStmt,            With.Parts(Skip.ToExpected<Expression>(),
                                                    Skip.ToZeroOrMore<Statement>(s => !(s is ElseBlock)),
                                                    Skip.ToZeroOrMore<ElseIfBlock>(),
                                                    Maybe.Node<FinalElseBlock>())
                                             .Make((ns, a, b, c, d) => new IfStatement(ns, a, new StatementList(ns, b), c, d)) },
            { Id.Rule.CallStmt,          With.Parts(Skip.ToExpected<AccessExpression>())
                                             .Make((ns, a) => new CallStatement(ns, a)) },
            { Id.Rule.SubCallStmt,       SubCallStmtNormalize.Then(With.Parts(GetSubCallExprItems)
                                                                       .Make((ns, i) => new InlineCallStatement(ns, CheckInlineCallArgs(new PathExpression(ns, i), ns)))) },
            { Id.Rule.AssignStmt,        With.Parts(Maybe.TokenOf(Id.Symbol.Set),
                                                    Skip.ToExpected<AccessExpression>(),
                                                    Skip.ToExpected<Expression>())
                                             .Make((ns, a, b, c) => a.Match(s  => new AssignStatement(ns, true,  b, c),
                                                                            () => new AssignStatement(ns, false, b, c))) },
            { Id.Rule.CaseStmtList,      SelectCaseTransform },
            { Id.Rule.SelectStmt,        With.Parts(Skip.ToExpected<Expression>(),
                                                    Skip.ToZeroOrMore<SelectCaseWhen>(),
                                                    Maybe.Node<SelectCaseElse>())
                                             .Make((ns, a, b, c) => new SelectStatement(ns, a, b, c)) },
            { Id.Rule.RedimDecl,         With.Parts(Expect.Node<Identifier>(),
                                                    Expect.TokenOf(Id.Symbol.OpenParen),
                                                    Expect.Node<ExpressionList>())
                                             .Make((ns, tid, top, exl) => new RedimVariable(ns, tid, exl)) },
            { Id.Rule.RedimStmt,         With.Condition((c, i) => i.Length > 2 && i[1] is TokenItem,
                                                        With.Parts(Expect.TokenOf(Id.Symbol.Redim),
                                                                   Expect.TokenOf(Id.Symbol.Preserve),
                                                                   Skip.ToOneOrMore<RedimVariable>())
                                                            .Make((ns, tr, tp, v) => new RedimStatement(ns, true, v)),
                                                        With.Parts(Expect.TokenOf(Id.Symbol.Redim),
                                                                   Skip.ToOneOrMore<RedimVariable>())
                                                            .Make((ns, tr, v) => new RedimStatement(ns, false, v))) },
            { Id.Rule.LoopStmt,          With.Condition((c, i) => i.Length > 0 && i[0] is TokenItem token && token.Value.Id.IsEqual(Id.Symbol.Do),
                                                        DoLoopTransform,
                                                        WhileTransform) },
            { Id.Rule.ForStmt,           With.Condition((c, i) => i.Length > 1 && i[1] is TokenItem token && token.Value.Id.IsEqual(Id.Symbol.Each),
                                                        ForEachTransform,
                                                        ForTransform) },
            { Id.Rule.WithStmt,          With.Parts(Expect.TokenOf(Id.Symbol.With),
                                                    Expect.Node<Expression>(),
                                                    Skip.ToZeroOrMore<Statement>())
                                             .Make((ns, t, e, s) => new WithStatement(ns, e, s)) },
            { Id.Rule.Program,           With.Parts(Skip.ToZeroOrMore<Statement>())
                                             .Make((ns, a) => new Program(ns, a)) }
        };

        public static Option<Transform> NodeTransform(this ParseId @this)
            => NodeBuilders.ContainsKey(@this) ? NodeBuilders[@this].AsOption() : Option<Transform>.None;


    }

}
