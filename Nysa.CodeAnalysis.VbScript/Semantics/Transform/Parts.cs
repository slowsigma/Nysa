using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class Parts
    {
        public Transform Return()
            => (n, m) => m;
        public Transform Make<TResult>(Func<SyntaxNode, TResult> how)
            where TResult : CodeNode
            => (n, m) => new TransformItem[] { (SemanticItem)how(n.Node) };
    }

    public class Parts<TA>
    {
        private Get<TA> A;

        public Parts(Get<TA> a) { A = a; }

        public Transform Make<TResult>(Func<SyntaxNode, TA, TResult> how)
            where TResult : CodeNode
            => (n, m) => new TransformItem[] { (SemanticItem)how(n.Node, A(m, Index.Start).Item) };

        public Transform Make(Func<SyntaxNode, TA, TransformItem[]> how)
            => (n, m) => how(n.Node, A(m, Index.Start).Item);
    }

    public class Parts<TA, TB>
    {
        private Get<TA> A;
        private Get<TB> B;

        public Parts(Get<TA> a, Get<TB> b) { A = a; B = b; }

        public Transform Make<TResult>(Func<SyntaxNode, TA, TB, TResult> how)
            where TResult : CodeNode
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);

                return new TransformItem[] { (SemanticItem)how(n.Node, pa.Item, pb.Item) };
            };

        public Transform Make(Func<SyntaxNode, TA, TB, TransformItem[]> how)
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);

                return how(n.Node, pa.Item, pb.Item);
            };
    }

    public class Parts<TA, TB, TC>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;

        public Parts(Get<TA> a, Get<TB> b, Get<TC> c) { A = a; B = b; C = c; }

        public Transform Make<TResult>(Func<SyntaxNode, TA, TB, TC, TResult> how)
            where TResult : CodeNode
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);
                var pc = C(m, pb.Remainder);

                return new TransformItem[] { (SemanticItem)how(n.Node, pa.Item, pb.Item, pc.Item) };
            };

        public Transform Make(Func<SyntaxNode, TA, TB, TC, TransformItem[]> how)
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);
                var pc = C(m, pb.Remainder);

                return how(n.Node, pa.Item, pb.Item, pc.Item);
            };
    }

    public class Parts<TA, TB, TC, TD>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;

        public Parts(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d) { A = a; B = b; C = c; D = d; }

        public Transform Make<TResult>(Func<SyntaxNode, TA, TB, TC, TD, TResult> how)
            where TResult : CodeNode
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);
                var pc = C(m, pb.Remainder);
                var pd = D(m, pc.Remainder);

                return new TransformItem[] { (SemanticItem)how(n.Node, pa.Item, pb.Item, pc.Item, pd.Item) };
            };

        public Transform Make<TResult>(Func<SyntaxNode, TA, TB, TC, TD, IEnumerable<TResult>> how)
            where TResult : CodeNode
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);
                var pc = C(m, pb.Remainder);
                var pd = D(m, pc.Remainder);

                return how(n.Node, pa.Item, pb.Item, pc.Item, pd.Item).Select(cn => (SemanticItem)cn).ToArray();
            };
    }

    public class Parts<TA, TB, TC, TD, TE>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;

        public Parts(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e) { A = a; B = b; C = c; D = d; E = e; }

        public Transform Make<TResult>(Func<SyntaxNode, TA, TB, TC, TD, TE, TResult> how)
            where TResult : CodeNode
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);
                var pc = C(m, pb.Remainder);
                var pd = D(m, pc.Remainder);
                var pe = E(m, pd.Remainder);

                return new TransformItem[] { (SemanticItem)how(n.Node, pa.Item, pb.Item, pc.Item, pd.Item, pe.Item) };
            };

        public Transform Make(Func<SyntaxNode, TA, TB, TC, TD, TE, TransformItem[]> how)
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);
                var pc = C(m, pb.Remainder);
                var pd = D(m, pc.Remainder);
                var pe = E(m, pd.Remainder);

                return how(n.Node, pa.Item, pb.Item, pc.Item, pd.Item, pe.Item);
            };
    }

    public class Parts<TA, TB, TC, TD, TE, TF>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;

        public Parts(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f) { A = a; B = b; C = c; D = d; E = e; F = f; }

        public Transform Make<TResult>(Func<SyntaxNode, TA, TB, TC, TD, TE, TF, TResult> how)
            where TResult : CodeNode
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);
                var pc = C(m, pb.Remainder);
                var pd = D(m, pc.Remainder);
                var pe = E(m, pd.Remainder);
                var pf = F(m, pe.Remainder);

                return new TransformItem[] { (SemanticItem)how(n.Node, pa.Item, pb.Item, pc.Item, pd.Item, pe.Item, pf.Item) };
            };
    }

    public class Parts<TA, TB, TC, TD, TE, TF, TG>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;

        public Parts(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; }

        public Transform Make<TResult>(Func<SyntaxNode, TA, TB, TC, TD, TE, TF, TG, TResult> how)
            where TResult : CodeNode
            => (n, m) =>
            {
                var pa = A(m, Index.Start);
                var pb = B(m, pa.Remainder);
                var pc = C(m, pb.Remainder);
                var pd = D(m, pc.Remainder);
                var pe = E(m, pd.Remainder);
                var pf = F(m, pe.Remainder);
                var pg = G(m, pf.Remainder);

                return new TransformItem[] { (SemanticItem)how(n.Node, pa.Item, pb.Item, pc.Item, pd.Item, pe.Item, pf.Item, pg.Item) };
            };
    }


}