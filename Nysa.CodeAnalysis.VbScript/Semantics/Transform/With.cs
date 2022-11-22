using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxToken = Nysa.Text.Lexing.Token;
using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class With
    {
        internal static readonly String MISSING_NODE         = "Error: expected node is missing.";
        internal static readonly String MISSING_TOKEN        = "Error: expected token is missing.";
        internal static readonly String MISSING_NODE_TOKEN   = "Error: expected node or token is missing.";
        internal static readonly String UNEXPECTED_NODE      = "Error: unexpected node.";

        public static Parts Parts() => new Parts();
        public static Parts<A> Parts<A>(Get<A> a) => new Parts<A>(a);
        public static Parts<A, B> Parts<A, B>(Get<A> a, Get<B> b) => new Parts<A, B>(a, b);
        public static Parts<A, B, C> Parts<A, B, C>(Get<A> a, Get<B> b, Get<C> c) => new Parts<A, B, C>(a, b, c);
        public static Parts<A, B, C, D> Parts<A, B, C, D>(Get<A> a, Get<B> b, Get<C> c, Get<D> d) => new Parts<A, B, C, D>(a, b, c, d);
        public static Parts<A, B, C, D, E> Parts<A, B, C, D, E>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e) => new Parts<A, B, C, D, E>(a, b, c, d, e);
        public static Parts<A, B, C, D, E, F> Parts<A, B, C, D, E, F>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f) => new Parts<A, B, C, D, E, F>(a, b, c, d, e, f);
        public static Parts<A, B, C, D, E, F, G> Parts<A, B, C, D, E, F, G>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g) => new Parts<A, B, C, D, E, F, G>(a, b, c, d, e, f, g);
        public static Either Either() => new Either();

        public static Transform Concat(Get<IEnumerable<TransformItem>> a, Get<IEnumerable<TransformItem>> b)
            => (c, i) =>
            {
                var ra = a(i, Index.Start);
                var rb = b(i, ra.Remainder);

                return ra.Item.Concat(rb.Item).ToArray();
            };

        public static Transform Concat<T, TResult>(Get<IEnumerable<T>> a, Get<IEnumerable<T>> b, Func<T, TResult> transform)
            where TResult : TransformItem
            => (c, i) =>
            {
                var ra = a(i, Index.Start);
                var rb = b(i, ra.Remainder);

                return ra.Item.Concat(rb.Item).Select(v => transform(v)).ToArray();
            };

        public static Condition Either<TResult>(Func<CodeNode, TResult> howWhenNode, Func<SyntaxNode, SyntaxToken, TResult> howWhenToken)
            where TResult : CodeNode
            => new Condition((c, i) => i[Index.Start] is SemanticItem,
                             (c, i) => i[Index.Start].Match(n => new TransformItem[] { (SemanticItem)howWhenNode(n) },
                                                            st => throw new Exception(With.MISSING_NODE)),
                             (c, i) => i[Index.Start].Match(n => throw new Exception(With.MISSING_TOKEN),
                                                            st => new TransformItem[] { (SemanticItem)howWhenToken(c.Node, st) }));

        public static Transform Condition(Func<TransformContext, TransformItem[], Boolean> predicate, Transform whenTrue, Transform whenFalse)
            => (c, i) => predicate(c, i) ? whenTrue(c, i) : whenFalse(c, i);

        public static Transform Optional(Transform whenPresent)
            => (c, i) => i.Length == 0 ? i : whenPresent(c, i);

    }

}