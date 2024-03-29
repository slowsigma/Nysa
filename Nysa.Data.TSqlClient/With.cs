﻿using System;

namespace Nysa.Data.TSqlClient
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
        public static Read<A, B, C, D, E, F, G, H, I, J, K, L> Row<A, B, C, D, E, F, G, H, I, J, K, L>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g, Get<H> h, Get<I> i, Get<J> j, Get<K> k, Get<L> l) => new Read<A, B, C, D, E, F, G, H, I, J, K, L>(a, b, c, d, e, f, g, h, i, j, k, l);
        public static Read<A, B, C, D, E, F, G, H, I, J, K, L, M> Row<A, B, C, D, E, F, G, H, I, J, K, L, M>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g, Get<H> h, Get<I> i, Get<J> j, Get<K> k, Get<L> l, Get<M> m) => new Read<A, B, C, D, E, F, G, H, I, J, K, L, M>(a, b, c, d, e, f, g, h, i, j, k, l, m);
        public static Read<A, B, C, D, E, F, G, H, I, J, K, L, M, N> Row<A, B, C, D, E, F, G, H, I, J, K, L, M, N>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g, Get<H> h, Get<I> i, Get<J> j, Get<K> k, Get<L> l, Get<M> m, Get<N> n) => new Read<A, B, C, D, E, F, G, H, I, J, K, L, M, N>(a, b, c, d, e, f, g, h, i, j, k, l, m, n);
        public static Read<A, B, C, D, E, F, G, H, I, J, K, L, M, N, O> Row<A, B, C, D, E, F, G, H, I, J, K, L, M, N, O>(Get<A> a, Get<B> b, Get<C> c, Get<D> d, Get<E> e, Get<F> f, Get<G> g, Get<H> h, Get<I> i, Get<J> j, Get<K> k, Get<L> l, Get<M> m, Get<N> n, Get<O> o) => new Read<A, B, C, D, E, F, G, H, I, J, K, L, M, N, O>(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o);

        public static TSqlScript Script(String value) => new TSqlScript(value);
        public static TSqlConnect Instance(String source) => new TSqlConnect(source);

        public static TSqlParameter ParameterInt(this String name, Int32? value) => new TSqlParameter(name, value, System.Data.SqlDbType.Int);
        public static TSqlParameter ParameterVarchar(this String name, String? value) => new TSqlParameter(name, value, System.Data.SqlDbType.VarChar);
        public static TSqlParameter ParameterText(this String name, String? value) => new TSqlParameter(name, value, System.Data.SqlDbType.Text);
        public static TSqlParameter ParameterNVarchar(this String name, String? value) => new TSqlParameter(name, value, System.Data.SqlDbType.NVarChar);
        public static TSqlParameter ParameterBit(this String name, Boolean? value) => new TSqlParameter(name, value, System.Data.SqlDbType.Bit);
        public static TSqlParameter ParameterDateTime(this String name, DateTime? value) => new TSqlParameter(name, value, System.Data.SqlDbType.DateTime);
        public static TSqlParameter ParameterVarBinary(this String name, Byte[]? value) => new TSqlParameter(name, value, System.Data.SqlDbType.VarBinary);
        public static TSqlParameter ParameterBigInt(this String name, Int64? value) => new TSqlParameter(name, value, System.Data.SqlDbType.Int);
        public static TSqlParameter ParameterTime(this String name, TimeSpan? value) => new TSqlParameter(name, value, System.Data.SqlDbType.Time);
        public static TSqlParameter ParameterReal(this String name, Single? value) => new TSqlParameter(name, value, System.Data.SqlDbType.Real);
        public static TSqlParameter ParameterFloat(this String name, Double? value) => new TSqlParameter(name, value, System.Data.SqlDbType.Float);

        public static TSqlProcedure Procedure(this String name) => new TSqlProcedure(name);
        public static TSqlProcedure Procedure(this String name, params TSqlParameter[] parameters) => new TSqlProcedure(name, parameters);
    }

}
