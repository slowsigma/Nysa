using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;

namespace Nysa.Data.TSqlClient
{

    public class Read<TA>
    {
        private Get<TA> A;
        internal Read(Get<TA> a) { A = a; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TResult> how) => r => how(A(r, 0));
    }

    public class Read<TA, TB>
    {
        private Get<TA> A;
        private Get<TB> B;
        internal Read(Get<TA> a, Get<TB> b) { A = a; B = b; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TResult> how) => r => how(A(r, 0), B(r, 1));
    }

    public class Read<TA, TB, TC>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c) { A = a; B = b; C = c; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2));
    }

    public class Read<TA, TB, TC, TD>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d) { A = a; B = b; C = c; D = d; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3));
    }

    public class Read<TA, TB, TC, TD, TE>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e) { A = a; B = b; C = c; D = d; E = e; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4));
    }

    public class Read<TA, TB, TC, TD, TE, TF>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f) { A = a; B = b; C = c; D = d; E = e; F = f; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG, TH>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        private Get<TH> H;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g, Get<TH> h) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TH, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6), H(r, 7));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG, TH, TI>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        private Get<TH> H;
        private Get<TI> I;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g, Get<TH> h, Get<TI> i) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TH, TI, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6), H(r, 7), I(r, 8));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        private Get<TH> H;
        private Get<TI> I;
        private Get<TJ> J;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g, Get<TH> h, Get<TI> i, Get<TJ> j) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; J = j; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6), H(r, 7), I(r, 8), J(r, 9));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        private Get<TH> H;
        private Get<TI> I;
        private Get<TJ> J;
        private Get<TK> K;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g, Get<TH> h, Get<TI> i, Get<TJ> j, Get<TK> k) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; J = j; K = k; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6), H(r, 7), I(r, 8), J(r, 9), K(r, 10));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TL>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        private Get<TH> H;
        private Get<TI> I;
        private Get<TJ> J;
        private Get<TK> K;
        private Get<TL> L;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g, Get<TH> h, Get<TI> i, Get<TJ> j, Get<TK> k, Get<TL> l) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; J = j; K = k; L = l; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TL, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6), H(r, 7), I(r, 8), J(r, 9), K(r, 10), L(r, 11));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TL, TM>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        private Get<TH> H;
        private Get<TI> I;
        private Get<TJ> J;
        private Get<TK> K;
        private Get<TL> L;
        private Get<TM> M;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g, Get<TH> h, Get<TI> i, Get<TJ> j, Get<TK> k, Get<TL> l, Get<TM> m) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; J = j; K = k; L = l; M = m; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TL, TM, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6), H(r, 7), I(r, 8), J(r, 9), K(r, 10), L(r, 11), M(r, 12));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TL, TM, TN>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        private Get<TH> H;
        private Get<TI> I;
        private Get<TJ> J;
        private Get<TK> K;
        private Get<TL> L;
        private Get<TM> M;
        private Get<TN> N;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g, Get<TH> h, Get<TI> i, Get<TJ> j, Get<TK> k, Get<TL> l, Get<TM> m, Get<TN> n) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; J = j; K = k; L = l; M = m; N = n; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TL, TM, TN, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6), H(r, 7), I(r, 8), J(r, 9), K(r, 10), L(r, 11), M(r, 12), N(r, 13));
    }

    public class Read<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TL, TM, TN, TO>
    {
        private Get<TA> A;
        private Get<TB> B;
        private Get<TC> C;
        private Get<TD> D;
        private Get<TE> E;
        private Get<TF> F;
        private Get<TG> G;
        private Get<TH> H;
        private Get<TI> I;
        private Get<TJ> J;
        private Get<TK> K;
        private Get<TL> L;
        private Get<TM> M;
        private Get<TN> N;
        private Get<TO> O;
        internal Read(Get<TA> a, Get<TB> b, Get<TC> c, Get<TD> d, Get<TE> e, Get<TF> f, Get<TG> g, Get<TH> h, Get<TI> i, Get<TJ> j, Get<TK> k, Get<TL> l, Get<TM> m, Get<TN> n, Get<TO> o) { A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; J = j; K = k; L = l; M = m; N = n; O = o; }
        public Func<SqlDataReader, TResult> Make<TResult>(Func<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ, TK, TL, TM, TN, TO, TResult> how) => r => how(A(r, 0), B(r, 1), C(r, 2), D(r, 3), E(r, 4), F(r, 5), G(r, 6), H(r, 7), I(r, 8), J(r, 9), K(r, 10), L(r, 11), M(r, 12), N(r, 13), O(r, 14));
    }

}
