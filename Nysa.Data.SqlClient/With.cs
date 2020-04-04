using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Nysa.Data.SqlClient
{

    public static class With
    {
        public static Read<A> Row<A>(Get<A> a) => new Read<A>(a);
        public static Read<A, B> Row<A, B>(Get<A> a, Get<B> b) => new Read<A, B>(a, b);
        public static Read<A, B, C> Row<A, B, C>(Get<A> a, Get<B> b, Get<C> c) => new Read<A, B, C>(a, b, c);
        public static Read<A, B, C, D> Row<A, B, C, D>(Get<A> a, Get<B> b, Get<C> c, Get<D> d) => new Read<A, B, C, D>(a, b, c, d);
        public static Read<A, B, C, D, E> Row<A, B, C, D, E>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e) => new Read<A, B, C, D, E>(a, b, c, d, e);
        public static Read<A, B, C, D, E, F> Row<A, B, C, D, E, F>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f) => new Read<A, B, C, D, E, F>(a, b, c, d, e, f);
        public static Read<A, B, C, D, E, F, G> Row<A, B, C, D, E, F, G>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g) => new Read<A, B, C, D, E, F, G>(a, b, c, d, e, f, g);
        public static Read<A, B, C, D, E, F, G, H> Row<A, B, C, D, E, F, G, H>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g, Get<H> h) => new Read<A, B, C, D, E, F, G, H>(a, b, c, d, e, f, g, h);
        public static Read<A, B, C, D, E, F, G, H, I> Row<A, B, C, D, E, F, G, H, I>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g, Get<H> h, Get<I> i) => new Read<A, B, C, D, E, F, G, H, I>(a, b, c, d, e, f, g, h, i);
        public static Read<A, B, C, D, E, F, G, H, I, J> Row<A, B, C, D, E, F, G, H, I, J>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g, Get<H> h, Get<I> i, Get<J> j) => new Read<A, B, C, D, E, F, G, H, I, J>(a, b, c, d, e, f, g, h, i, j);
        public static Read<A, B, C, D, E, F, G, H, I, J, K> Row<A, B, C, D, E, F, G, H, I, J, K>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g, Get<H> h, Get<I> i, Get<J> j, Get<K> k) => new Read<A, B, C, D, E, F, G, H, I, J, K>(a, b, c, d, e, f, g, h, i, j, k);
    }

}
